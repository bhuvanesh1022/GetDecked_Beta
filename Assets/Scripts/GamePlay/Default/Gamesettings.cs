using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
[CreateAssetMenu (menuName = "Gamesettings")]
public class Gamesettings : ScriptableObject
{
    public static Gamesettings _gamesettings;
    public int playerenteredindex;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

   
}
