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
    private void Awake() {
        controller = FindObjectOfType<Controller>();
        wages = GameObject.FindGameObjectWithTag("Wages").GetComponent<WagesManager>();
    

    }

    public void OnDrop(PointerEventData eventData) {
        RectTransform rect = transform as RectTransform;
       Obj._PlayerCardVal = true;
       // ItemDragHandler.itemBeingDragged.transform.gameObject.SetActive(false);
        controller._CardPlacing();
    }
   

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            
        }
        else if (stream.IsReading) {
            
        }
    }
}
