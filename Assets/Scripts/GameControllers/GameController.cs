using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public PhotonView pv;
    public GameObject myPlayer;
    public GameplayManager _gameplayManager;
    public DataController _datacontroller;
    public AudioSource BG_AudioSource;

    public void Awake()
    {
        _datacontroller = GameObject.FindWithTag("DataController").GetComponent<DataController>();
        BG_AudioSource = GameObject.FindWithTag("DataController").GetComponent<AudioSource>();
    }

    void Start()
    {
        myPlayer = PhotonNetwork.Instantiate(_datacontroller.myCharacter, Vector3.zero, Quaternion.identity);
        myPlayer.GetComponent<PlayerController>().myName = _datacontroller.myName;
        _gameplayManager.myCardHolder.myPlayer = myPlayer.GetComponent<PlayerController>();
    }

    public void ReloadApp()
    {
        BG_AudioSource.Stop();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public void CloseApp()
    {
        Application.Quit();
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
            _gameplayManager.AvailableBet[i].text = _gameplayManager.MaxBet.ToString();
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
                    _gameplayManager.playedCards[0].sprite = _gameplayManager.cards[_gameplayManager.playerList[i].placedCard];
                    _gameplayManager.playedCards[0].enabled = true;
                }
                else
                {
                    _gameplayManager.playedCards[1].sprite = _gameplayManager.ReadyCard;
                    _gameplayManager.playedCards[1].enabled = true;
                }

                if (!_gameplayManager.playersPlayed.Contains(_gameplayManager.playerList[i]))
                {
                    _gameplayManager.playersPlayed.Add(_gameplayManager.playerList[i]);
                }
            }
        }

        if (_gameplayManager.playerList.Count == _gameplayManager.playersPlayed.Count)
        {
            for (int i = 0; i < _gameplayManager.playersPlayed.Count; i++)
            {
                for (int j = 1; j < _gameplayManager.playersPlayed.Count; j++)
                {
                    if (_gameplayManager.playersPlayed[i].placedCard == 0 &&_gameplayManager.playersPlayed[j].placedCard == 0)
                    {
                        Debug.Log("sameCards");
                    }
                    else if (_gameplayManager.playersPlayed[i].placedCard == 1 && _gameplayManager.playersPlayed[j].placedCard == 1)
                    {
                        Debug.Log("sameCards");
                    }
                    else if (_gameplayManager.playersPlayed[i].placedCard == 2 && _gameplayManager.playersPlayed[j].placedCard == 2)
                    {
                        Debug.Log("sameCards");
                    }
                    else if (_gameplayManager.playersPlayed[i].placedCard == 0 && _gameplayManager.playersPlayed[j].placedCard == 1)
                    {
                        _gameplayManager.playersPlayed[i].isWon = false;
                        _gameplayManager.playersPlayed[j].isWon = true;
                        Debug.Log(_gameplayManager.playersPlayed[j].myName + " is won this turn...");
                    }
                    else if (_gameplayManager.playersPlayed[i].placedCard == 0 && _gameplayManager.playersPlayed[j].placedCard == 2)
                    {
                        _gameplayManager.playersPlayed[i].isWon = true;
                        _gameplayManager.playersPlayed[j].isWon = false;
                        Debug.Log(_gameplayManager.playersPlayed[i].myName + " is won this turn...");
                    }
                    else if (_gameplayManager.playersPlayed[i].placedCard == 1 && _gameplayManager.playersPlayed[j].placedCard == 2)
                    {
                        _gameplayManager.playersPlayed[i].isWon = false;
                        _gameplayManager.playersPlayed[j].isWon = true;
                        Debug.Log(_gameplayManager.playersPlayed[j].myName + " is won this turn...");
                    }
                    else if (_gameplayManager.playersPlayed[i].placedCard == 1 && _gameplayManager.playersPlayed[j].placedCard == 0)
                    {
                        _gameplayManager.playersPlayed[i].isWon = true;
                        _gameplayManager.playersPlayed[j].isWon = false;
                        Debug.Log(_gameplayManager.playersPlayed[i].myName + " is won this turn...");
                    }
                    else if (_gameplayManager.playersPlayed[i].placedCard == 2 && _gameplayManager.playersPlayed[j].placedCard == 0)
                    {
                        _gameplayManager.playersPlayed[i].isWon = false;
                        _gameplayManager.playersPlayed[j].isWon = true;
                        Debug.Log(_gameplayManager.playersPlayed[j].myName + " is won this turn...");
                    }
                    else if (_gameplayManager.playersPlayed[i].placedCard == 2 && _gameplayManager.playersPlayed[j].placedCard == 1)
                    {
                        _gameplayManager.playersPlayed[i].isWon = true;
                        _gameplayManager.playersPlayed[j].isWon = false;
                        Debug.Log(_gameplayManager.playersPlayed[i].myName + " is won this turn...");
                    }
                }
            }

            for (int i = 0; i < _gameplayManager.playersPlayed.Count; i++)
            {
                _gameplayManager.playersPlayed[i].cardPlaced = false;
                _gameplayManager.playersPlayed[i].placedCard = -1;
                _gameplayManager.playersPlayed[i].isWon = false;
                _gameplayManager.playedCards[i].enabled = false;
            }
            _gameplayManager.playersPlayed.Clear();
        }
    }
}
