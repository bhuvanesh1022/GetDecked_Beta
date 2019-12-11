using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]

public class PlayerController : MonoBehaviour, IPunObservable
{
    public PhotonView pv;
    public string myName;
    public bool cardPlaced;
    public int placedCard;

    private GameplayManager _gameplayManager;
    private GameController _gameController;
    private PlayerHealthManager _healthManager;
    private PlayerBetManager _betManager;
    private PlayerSpecialManager _specialManager;

    public bool isWon;

    public void Awake()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        _gameplayManager = GameObject.FindWithTag("GameplayManager").GetComponent<GameplayManager>();

        _healthManager = GetComponent<PlayerHealthManager>();
        _betManager = GetComponent<PlayerBetManager>();
        _specialManager = GetComponent<PlayerSpecialManager>();
    }

    void Start()
    {
        if (!_gameplayManager.playerList.Contains(this))
        {
            _gameplayManager.playerList.Add(this);
        }
        _gameController.LoadAvatar();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(myName);
            stream.SendNext(cardPlaced);
            stream.SendNext(placedCard);
            stream.SendNext(isWon);
        }
        if (stream.IsReading)
        {
            myName = (string)stream.ReceiveNext();
            cardPlaced = (bool)stream.ReceiveNext();
            placedCard = (int)stream.ReceiveNext();
            isWon = (bool)stream.ReceiveNext();
        }
    }
}
