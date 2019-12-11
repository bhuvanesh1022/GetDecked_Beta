using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class ItemDropHandler : MonoBehaviourPunCallbacks, IDropHandler,IPunObservable {
    public Controller controller;
    public WagesManager wages;
    public bool _CardPlaced;
    private void Awake() {
        controller = FindObjectOfType<Controller>();
        wages = GameObject.FindGameObjectWithTag("Wages").GetComponent<WagesManager>();
    }

    public void OnDrop(PointerEventData eventData) {
        RectTransform rect = transform as RectTransform;  
      
        //controller._ISPlacedCard = true;
        _CardPlaced = true;
        photonView.RPC("Placecard", RpcTarget.AllBuffered, null);
    }
    [PunRPC]
    public void Placecard() {
        if (controller._IsBetActive == true) {
            ItemDragHandler.itemBeingDragged.transform.gameObject.SetActive(false);
            controller._IsBetActive = false;
            if (photonView.IsMine) {

                controller._PlacedCardPos[0].SetActive(true);
                controller._PlacedCardPos[0].GetComponent<Image>().sprite = controller._PlaceCardSprite[controller._CardCnt];
                controller.PlayerOutLine[0].SetActive(false);
                controller._PlaceCardTxt[0].GetComponent<TextMeshProUGUI>().text = wages._FinalBet.ToString();
            }
            else {
                controller._PlacedCardPos[0].SetActive(true);
                controller._PlacedCardPos[0].GetComponent<Image>().sprite = controller._PlaceCardSprite[controller._CardCnt];
                controller.PlayerOutLine[0].SetActive(false);
                controller._PlaceCardTxt[0].GetComponent<TextMeshProUGUI>().text = wages._FinalBet.ToString();
            }

            //print("hellow-------");
            //if (controller._PlaceCardList.Contains(controller._PlacedCardPos[0])) {
            //    return;
            //}
            //controller._PlaceCardList.Add(controller._PlacedCardPos[0]);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(_CardPlaced);
        }
        else if (stream.IsReading) {
            _CardPlaced = (bool)stream.ReceiveNext();
        }
    }
}
