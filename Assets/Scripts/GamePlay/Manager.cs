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

    private void Awake() 
    {
       // DC = GameObject.FindGameObjectWithTag("DataController").GetComponent<DataController>();
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject _Player = PhotonNetwork.Instantiate("Myplayer", Vector3.zero, Quaternion.identity);
        //GameObject myAvatar = PhotonNetwork.Instantiate("localCard", Vector3.zero, Quaternion.identity);   // Player Card position
        //if (controller.photonView.IsMine) {
        //    print("posit---------");
        //    myAvatar.transform.parent = playerPanel;
        //    myAvatar.transform.localPosition = Vector3.zero;

        //}
          
    }

   
}
