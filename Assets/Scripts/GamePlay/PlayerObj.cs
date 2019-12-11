using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class PlayerObj : MonoBehaviourPunCallbacks,IPunObservable
{
    public Controller controller;
    public Manager Mg;
   // public RoomController room;
    public DataController DC;
    public int PLId;
    public PhotonView pv;
    public string avatarName;
    //
    public bool _PlayerCardVal;

    private void Awake() {
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
        controller.Obj = this;
        DC = GameObject.FindGameObjectWithTag("DataController").GetComponent<DataController>();
        //room = GameObject.FindGameObjectWithTag("RoomController").GetComponent<RoomController>();
        Mg = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
    }
    void Start()
    {
        //PLId = DC.MyId;
        _PlayerPosition();
        _PlayerDetails();
        photonView.RPC("Addplayers",RpcTarget.AllBuffered,null);
    }
    void _PlayerDetails() {
        PLId = DC.MyId;
        if (pv.IsMine) {
            avatarName = pv.Owner.NickName;
            this.gameObject.name = pv.Owner.NickName;
            Mg.myName.text = avatarName;
            
        }
        else {
            avatarName = pv.Owner.NickName;
            this.gameObject.name = pv.Owner.NickName;
            Mg.myName1.text = avatarName;           
        }
        }
    void _PlayerPosition() {
        for (int i = 0; i < controller._PlayerPos.Length; i++) {
            if (pv.IsMine) {
                print("1---------->");
                transform.parent = controller._PlayerPos[0].transform;
                transform.localPosition = Vector3.zero;
                transform.GetComponent<Image>().sprite = controller._PlayerSprite[(int)PhotonNetwork.LocalPlayer.CustomProperties["Avatar"]];//(int)PhotonNetwork.LocalPlayer.CustomProperties["Avatar"]
                print(" DC.MyId ----" + (int)PhotonNetwork.LocalPlayer.CustomProperties["Avatar"]);
            }
            else {
                print("2---------->");
                transform.parent = controller._PlayerPos[1].transform;
                transform.localPosition = Vector3.zero;
                transform.GetComponent<Image>().sprite = controller._PlayerSprite[(int)PhotonNetwork.LocalPlayer.CustomProperties["Avatar"]];
                print(" DC.MyId ----" + (int)PhotonNetwork.LocalPlayer.CustomProperties["Avatar"]);
            }
        }     
    }
    

    [PunRPC]
    public void Addplayers() {
        if (controller._PlayerList.Contains(gameObject)) {
            return;
        }
        controller._PlayerList.Add(gameObject);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(_PlayerCardVal);
        }
        else if (stream.IsReading) {
            _PlayerCardVal = (bool)stream.ReceiveNext();
        }
    }
}
