using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class SpecialManager : MonoBehaviourPunCallbacks
{
    public Controller controller;
    public GameObject _SpCard;
    int SpecialCount;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
    }
    private void Update() {
        SpecialCardFun();
    }
    void SpecialCardFun() {
        if (controller._PlayerList[0].GetComponent<PlayerObj>()._SpecialCardActive && photonView.IsMine) {
            _SpCard.GetComponent<CanvasGroup>().interactable = false;
        }
        }
    // special button
    public void Click_specialFun() {
        _SpCard.GetComponent<CanvasGroup>().interactable=false;
        for (int i = 0; i < controller._PlayerList.Count; i++) {
            controller._PlayerList[i].GetComponent<PlayerObj>()._SpecialCardActive = true;
        }
        //for (int i=0;i<controller._specialIcons.Length;i++) {
        //   controller._specialIcons[0].GetComponent<Image>().color=Color.gray;
       // }
        photonView.RPC("AddSpecialCard",RpcTarget.AllBuffered,null);
    }

    [PunRPC]
   public void AddSpecialCard() {
        for (int i = 0; i < controller._PlayerList.Count; i++) {
            if (controller._PlayerList[i].GetComponent<PlayerObj>().pv.IsMine && controller._PlayerList[i].GetComponent<PlayerObj>()._SpecialCardActive) {
                print("1-----------");
               controller._specialIcons[i].GetComponent<Image>().color = Color.gray;
            }
            else {
                print("2-----------");
                controller._specialIcons[i].GetComponent<Image>().color = Color.gray;
            }
        }

    }
   


}
