using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]

public class PlayerController : MonoBehaviour, IPunObservable
{
    public PhotonView pv;
    public string myName;
    public bool canPlaceCard;
    public bool cardPlaced;
    public int placedCard;
    public bool isWon;
    public PlayerHealthManager _healthManager;
    public PlayerBetManager _betManager;
    public PlayerSpecialManager _specialManager;

    private GameplayManager _gameplayManager;
    private GameController _gameController;


    public void Awake()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        _gameplayManager = GameObject.FindWithTag("GameplayManager").GetComponent<GameplayManager>();
    }

    void Start()
    {
        if (!_gameplayManager.playerList.Contains(this))
        {
            _gameplayManager.playerList.Add(this);
        }
        _gameController.LoadAvatar();
    }

    public void Update()
    {
        _gameController.gameEnd |= _healthManager.currentHealth <= 0;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(myName);
            stream.SendNext(canPlaceCard);
            stream.SendNext(cardPlaced);
            stream.SendNext(placedCard);
            stream.SendNext(isWon);
        }
        if (stream.IsReading)
        {
            myName = (string)stream.ReceiveNext();
            canPlaceCard = (bool)stream.ReceiveNext();
            cardPlaced = (bool)stream.ReceiveNext();
            placedCard = (int)stream.ReceiveNext();
            isWon = (bool)stream.ReceiveNext();
        }
    }
}
