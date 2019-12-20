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
    public DataController DC;
    public WagesManager Wage;

    public PhotonView pv;
    public string avatarName;
    public GameObject healthbar;
    public GameObject _TokenTxt;
    public int currentBet;
    //
    public bool _PlacedCard, _placedBet;
    public int CardId;
    public bool _IsplayerWin,_IsSameCard;
    public float currentHealth;
    public bool updateHealth;
    public bool canUpdateHealth;
    //opponent bet
    public int _RemainingBet;

    [Header("Special Details")]
    public bool _SpecialCardActive;

    private void Awake() {
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
        if (photonView.IsMine) {
            controller.Obj = this;
        }
        DC = GameObject.FindGameObjectWithTag("DataController").GetComponent<DataController>();
        Mg = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        Wage = GameObject.FindGameObjectWithTag("Wages").GetComponent<WagesManager>();
        // controller.wages.Obj = this;
    }
    void Start()
    {
        canUpdateHealth = true;
        _RemainingBet = Wage._MaxBetValue;
        _PlayerPosition();
        _PlayerDetails();
        photonView.RPC("Addplayers",RpcTarget.AllBuffered,null);
    }

    public void Update() {
        if (updateHealth && canUpdateHealth) {
            currentHealth -= Wage._Health;
            //healthbar.GetComponent<Image>().fillAmount = currentHealth / controller._MaxHealth;
            print("show val"+ healthbar.GetComponent<Image>().fillAmount+ currentHealth);
            canUpdateHealth = false;
            updateHealth = false;
        }
       
    }
   
    void _PlayerDetails() {
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
                healthbar = controller.HealthLoader[0];//health
                _TokenTxt = controller.AvailableToken[0].gameObject;
                transform.localPosition = Vector3.zero;
                transform.GetComponent<Image>().sprite = controller._PlayerSprite[(int)PhotonNetwork.LocalPlayer.CustomProperties["Avatar"]];//(int)PhotonNetwork.LocalPlayer.CustomProperties["Avatar"]
                print(" DC.MyId ----" + (int)PhotonNetwork.LocalPlayer.CustomProperties["Avatar"]);
            }
            else {
                print("2---------->");
                transform.parent = controller._PlayerPos[1].transform;
                healthbar = controller.HealthLoader[1];
                _TokenTxt = controller.AvailableToken[1].gameObject;
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
            stream.SendNext(_PlacedCard);
            stream.SendNext(_placedBet);
            stream.SendNext(CardId);
            stream.SendNext(_IsplayerWin);
            stream.SendNext(_IsSameCard);
            stream.SendNext(currentBet);
            stream.SendNext(_RemainingBet);
            stream.SendNext(currentHealth);
            stream.SendNext(updateHealth);
            stream.SendNext(canUpdateHealth);
        }
        else if (stream.IsReading) {
            _PlacedCard = (bool)stream.ReceiveNext();
            _placedBet = (bool)stream.ReceiveNext();
            CardId = (int)stream.ReceiveNext();
            _IsplayerWin = (bool)stream.ReceiveNext();
            _IsSameCard = (bool)stream.ReceiveNext();
            currentBet = (int)stream.ReceiveNext();
            _RemainingBet = (int)stream.ReceiveNext();
            currentHealth = (float)stream.ReceiveNext();
            updateHealth = (bool)stream.ReceiveNext();
            canUpdateHealth = (bool)stream.ReceiveNext();
        }
    }
}
