using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameController : MonoBehaviour
{
    public PhotonView pv;
    public GameObject myPlayer;
    public int MaxBet;
    public GameplayManager _gameplayManager;
    public DataController _datacontroller;

    public void Awake()
    {
        _datacontroller = GameObject.FindWithTag("DataController").GetComponent<DataController>();
    }

    void Start()
    {
        myPlayer = PhotonNetwork.Instantiate(_datacontroller.myCharacter, Vector3.zero, Quaternion.identity);
        myPlayer.GetComponent<PlayerController>().myName = _datacontroller.myName;
        _gameplayManager.myCardHolder.myPlayer = myPlayer.GetComponent<PlayerController>();
    }

    public void LoadAvatar()
    {
        for (int i = 0; i < _gameplayManager.playerList.Count; i++)
        {
            if (_gameplayManager.playerList[i].pv.IsMine)
            {
                _gameplayManager.playerList[i].transform.parent = _gameplayManager.playerPanels[i].transform;
                _gameplayManager.playerNames[i].text = _gameplayManager.playerList[i].myName;
            }
            else
            {
                _gameplayManager.playerList[i].transform.parent = _gameplayManager.playerPanels[i].transform;
                _gameplayManager.playerNames[i].text = _gameplayManager.playerList[i].myName;
            }

            _gameplayManager.playerList[i].transform.localPosition = Vector3.zero;
            _gameplayManager.AvailableBet[i].text = MaxBet.ToString();
            _gameplayManager.HealthBar[i].fillAmount = 1.0f;
            _gameplayManager.Specials[i].enabled = true;
        }
    }

    public void PlaceCards()
    {
        pv.RPC("PlacingCards", RpcTarget.AllBuffered, null);
    }

    [PunRPC]
    public void PlacingCards()
    {
        StartCoroutine(CardsPlacing());
    }

    IEnumerator CardsPlacing()
    {
        for (int i = 0; i < _gameplayManager.playerList.Count; i++)
        {
            yield return new WaitForSeconds(.1f);

            if (_gameplayManager.playerList[i].cardPlaced)
            {
                if (_gameplayManager.playerList[i].pv.IsMine)
                {
                    _gameplayManager.myCard.sprite = _gameplayManager.cards[_gameplayManager.playerList[i].placedCard];
                    _gameplayManager.myCard.enabled = true;
                }
                else
                {
                    _gameplayManager.opponentCard.sprite = _gameplayManager.ReadyCard;
                    _gameplayManager.opponentCard.enabled = true;
                }
            }

        }
    }
}
