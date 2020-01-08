using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBetManager : MonoBehaviourPun, IPunObservable
{
    public PhotonView pv;
    public bool canBet;
    public bool betPlaced;
    public int placedBet;
    public int maxBetAvailable;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(canBet);
            stream.SendNext(betPlaced);
            stream.SendNext(placedBet);
            stream.SendNext(maxBetAvailable);
        }
        if (stream.IsReading)
        {
            canBet = (bool)stream.ReceiveNext();
            betPlaced = (bool)stream.ReceiveNext();
            placedBet = (int)stream.ReceiveNext();
            maxBetAvailable = (int)stream.ReceiveNext();
        }
    }

}
