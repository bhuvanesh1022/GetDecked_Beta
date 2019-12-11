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

    private void Awake() 
    {
       // controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
        
    }
    void Start()
    {
         GameObject _Player = PhotonNetwork.Instantiate("Myplayer", Vector3.zero, Quaternion.identity);
        _DropManager.Obj = _Player.GetComponent<PlayerObj>();
          
    }

   
}
