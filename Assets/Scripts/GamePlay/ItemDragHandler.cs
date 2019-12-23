using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;


public class ItemDragHandler : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler,IPunObservable
{
    public static ItemDragHandler itemDragHandler;
    public Controller controller;

    public static Transform itemBeingDragged;
    public Vector3 startPosition;
    public Transform startParent;
    private CanvasGroup canvasGroup;
    public int Cnt;
    private void Awake() {
        controller = FindObjectOfType<Controller>();
    }
    private void Start() {
        startPosition = transform.position;
        startParent = transform.parent;
    }
    

    public void OnBeginDrag(PointerEventData eventData) {
       if (controller._IsBetActive == true) {
            //controller._CardCnt = int.Parse(this.name.Split('-')[0]);
            controller._CardCnt = Cnt;
            itemBeingDragged = transform;
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (controller._IsBetActive == true) {
            Vector3 offsetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(offsetPos.x, offsetPos.y, 0);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        print("hellow--yyy-----");

       if (controller._IsBetActive == true) {

            canvasGroup.blocksRaycasts = true;
            if (transform.parent == startParent) {
                transform.position = startPosition;
            }
            else {
                transform.localPosition = Vector3.zero;
            }

            itemBeingDragged = null;
       }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(Cnt);
           }
        else if (stream.IsReading) {
            Cnt = (int)stream.ReceiveNext();
        }
    }
}
