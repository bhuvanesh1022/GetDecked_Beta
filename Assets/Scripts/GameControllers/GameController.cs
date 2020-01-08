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
    public bool canResolve;
    public bool revealCards;
    public bool gameEnd;

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
                _gameplayManager.playerList[i]._healthManager.healthBar = _gameplayManager.HealthBar[0];
                _gameplayManager.playerList[i]._healthManager.healthBar.fillAmount = 1;
                _gameplayManager.playerNames[i].text = _gameplayManager.playerList[i].myName;
            }
            else
            {
                _gameplayManager.playerList[i].transform.parent = _gameplayManager.playerPanels[i].transform;
                _gameplayManager.playerList[i]._healthManager.healthBar = _gameplayManager.HealthBar[1];
                _gameplayManager.playerList[i]._healthManager.healthBar.fillAmount = 1;
                _gameplayManager.playerNames[i].text = _gameplayManager.playerList[i].myName;
            }

            _gameplayManager.playerList[i].transform.localPosition = Vector3.zero;
            _gameplayManager.AvailableBet[i].text = _gameplayManager.MaxBet.ToString();
            _gameplayManager.playerList[i]._betManager.maxBetAvailable = _gameplayManager.MaxBet;

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
        yield return new WaitForSeconds(.1f);

        for (int i = 0; i < _gameplayManager.playerList.Count; i++)
        {
            if (_gameplayManager.playerList[i].cardPlaced && _gameplayManager.playerList[i].canPlaceCard)
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
                yield return new WaitForSeconds(.1f);
                _gameplayManager.playerList[i].canPlaceCard = false;
            }
        }

        if (_gameplayManager.playerList.Count == _gameplayManager.playersPlayed.Count)
        {
            revealCards = true;
            StartCoroutine(RevealCards());
        }
    }

    IEnumerator RevealCards()
    {
        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < _gameplayManager.playersPlayed.Count; i++)
        {
            if (!_gameplayManager.playersPlayed[i].pv.IsMine)
            {
                _gameplayManager.playedCards[1].sprite = _gameplayManager.cards[_gameplayManager.playersPlayed[i].placedCard];
            }
        }

        StartCoroutine(ResolveNow());
        canResolve = true;
    }

    IEnumerator ResolveNow()
    {
        canResolve = false;
       
        yield return new WaitForSeconds(2.0f);

        if (_gameplayManager.playersPlayed[0].placedCard == 0 && _gameplayManager.playersPlayed[1].placedCard == 0)
        {
            Debug.Log("sameCards with Attack");
        }
        else if (_gameplayManager.playersPlayed[0].placedCard == 1 && _gameplayManager.playersPlayed[1].placedCard == 1)
        {
            Debug.Log("sameCards with Defence");
        }
        else if (_gameplayManager.playersPlayed[0].placedCard == 2 && _gameplayManager.playersPlayed[1].placedCard == 2)
        {
            Debug.Log("sameCards with Throw");
        }
        else if (_gameplayManager.playersPlayed[0].placedCard == 0 && _gameplayManager.playersPlayed[1].placedCard == 1)
        {
            _gameplayManager.playersPlayed[0].isWon = false;
            _gameplayManager.playersPlayed[1].isWon = true;

        }
        else if (_gameplayManager.playersPlayed[0].placedCard == 0 && _gameplayManager.playersPlayed[1].placedCard == 2)
        {
            _gameplayManager.playersPlayed[0].isWon = true;
            _gameplayManager.playersPlayed[1].isWon = false;
        }
        else if (_gameplayManager.playersPlayed[0].placedCard == 1 && _gameplayManager.playersPlayed[1].placedCard == 2)
        {
            _gameplayManager.playersPlayed[0].isWon = false;
            _gameplayManager.playersPlayed[1].isWon = true;
        }
        else if (_gameplayManager.playersPlayed[0].placedCard == 1 && _gameplayManager.playersPlayed[1].placedCard == 0)
        {
            _gameplayManager.playersPlayed[0].isWon = true;
            _gameplayManager.playersPlayed[1].isWon = false;
        }
        else if (_gameplayManager.playersPlayed[0].placedCard == 2 && _gameplayManager.playersPlayed[1].placedCard == 0)
        {
            _gameplayManager.playersPlayed[0].isWon = false;
            _gameplayManager.playersPlayed[1].isWon = true;
        }
        else if (_gameplayManager.playersPlayed[0].placedCard == 2 && _gameplayManager.playersPlayed[1].placedCard == 1)
        {
            _gameplayManager.playersPlayed[0].isWon = true;
            _gameplayManager.playersPlayed[1].isWon = false;
        }

        StartCoroutine(UpdateHealth());
    }

    IEnumerator UpdateHealth()
    {
        for (int i = 0; i < _gameplayManager.playersPlayed.Count; i++)
        {
            for (int j = i+1; j < _gameplayManager.playersPlayed.Count; j++)
            {
                int pain;
                if (_gameplayManager.playersPlayed[i].isWon)
                {
                    pain = _gameplayManager.playersPlayed[i]._betManager.placedBet + 1;
                    _gameplayManager.playersPlayed[j]._healthManager.currentHealth -= pain;
                    _gameplayManager.playersPlayed[j]._healthManager.updateHealth = true;
                }
                else
                {
                    pain = _gameplayManager.playersPlayed[j]._betManager.placedBet + 1;
                    _gameplayManager.playersPlayed[i]._healthManager.currentHealth -= pain;
                    _gameplayManager.playersPlayed[i]._healthManager.updateHealth = true;
                }
                pain = 0;
            }
        }
        yield return new WaitForSeconds(.1f);

        StartCoroutine(ResetForNextRound());
    }

    IEnumerator ResetForNextRound()
    {
        if (!gameEnd)
        {
            for (int i = 0; i < _gameplayManager.playersPlayed.Count; i++)
            {
                _gameplayManager.playersPlayed[i].canPlaceCard = true;
                _gameplayManager.playersPlayed[i].cardPlaced = false;
                _gameplayManager.playersPlayed[i].placedCard = -1;
                _gameplayManager.playersPlayed[i]._betManager.placedBet = 0;
                _gameplayManager.playersPlayed[i]._betManager.betPlaced = false;
                _gameplayManager.playersPlayed[i].isWon = false;
                _gameplayManager.playedCards[i].enabled = false;
            }
            _gameplayManager.playersPlayed.Clear();
            yield return new WaitForSeconds(.1f);
            _gameplayManager.BetEnableBtn.SetActive(true);
            _gameplayManager.CardsPanel.SetActive(false);
        }
        else
        {
            StartCoroutine(EndCard());
        }
    }

    IEnumerator EndCard()
    {
        yield return new WaitForSeconds(.1f);

        for (int i = 0; i < _gameplayManager.playersPlayed.Count; i++)
        {
            if (_gameplayManager.playersPlayed[i]._healthManager.currentHealth > 0)
            {
                Debug.Log(_gameplayManager.playersPlayed[i].myName + " has won!");
            }
        }
    }
}
