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
    public PlayerObj Obj;
    public ItemDragHandler card;
    public bool _CardDrop;

    private void Awake() {
        controller = FindObjectOfType<Controller>();
        wages = GameObject.FindGameObjectWithTag("Wages").GetComponent<WagesManager>();
    }

    public void OnDrop(PointerEventData eventData) {
        RectTransform rect = transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
        {
            _CardDrop = true;
            Obj._PlacedCard = true;
            card = ItemDragHandler.itemBeingDragged.GetComponent<ItemDragHandler>();

            Obj.CardId = card.Cnt;

            // ItemDragHandler.itemBeingDragged.transform.gameObject.SetActive(false);
            controller._CardPlacing();// first
        }
    }
   

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(_CardDrop);
        }
        else if (stream.IsReading) {
            _CardDrop = (bool)stream.ReceiveNext();
        }
    }
}
