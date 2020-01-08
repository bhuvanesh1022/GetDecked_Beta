using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Manager : MonoBehaviourPunCallbacks
{
    //public DataController DC;
    public Controller controller;

    public TextMeshProUGUI myName, myName1;
    public TextMeshProUGUI opponentName;
    public Transform playerPanel;
    public ItemDropHandler _DropManager;
    [Header("Health details")]
    public List<GameObject> _HealthBar = new List<GameObject>();




    void Start()
    {
         GameObject _Player = PhotonNetwork.Instantiate("Myplayer", Vector3.zero, Quaternion.identity);
        _DropManager.Obj = _Player.GetComponent<PlayerObj>();
        //if (_Player.GetComponent<PlayerObj>().pv.IsMine) {
        //    _Player.GetComponent<PlayerObj>().healthbar = _HealthBar[0];
        //}
        //else {
        //    _Player.GetComponent<PlayerObj>().healthbar = _HealthBar[1];
        //}

        //GameObject _HealthBar = PhotonNetwork.Instantiate("Health", Vector3.zero, Quaternion.identity);
        //_DropManager.Obj = _Player.GetComponent<PlayerObj>();
    }

   
}
