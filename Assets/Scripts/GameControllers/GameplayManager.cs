using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameplayManager : MonoBehaviour
{
    public PhotonView pv;
    public List<PlayerController> playerList = new List<PlayerController>();
    public List<GameObject> playerPanels = new List<GameObject>();
    public List<TextMeshProUGUI> playerNames = new List<TextMeshProUGUI>();

    public int MaxBet;
    public TextMeshProUGUI maxBet;
    public List<TextMeshProUGUI> AvailableBet = new List<TextMeshProUGUI>();
    public TextMeshProUGUI currentBet;
    public GameObject BetSlider;
    public GameObject BetEnableBtn;

    public List<Image> HealthBar = new List<Image>();
    public List<Image> Specials = new List<Image>();

    public GameObject CardsPanel;
    public List<Sprite>cards = new List<Sprite>();
    public DropManager myCardHolder;
    public List<Image> playedCards = new List<Image>();
    public Sprite ReadyCard;

    public List<PlayerController> playersPlayed = new List<PlayerController>();

    public GameController _gameController;

    public void Update()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].pv.IsMine)
            {
                BetSlider.GetComponentInChildren<Slider>().value = Mathf.Clamp(BetSlider.GetComponentInChildren<Slider>().value, 0, playerList[i]._betManager.maxBetAvailable);
            }
        }
    }

    public void PrepareForBet()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].pv.IsMine)
            {
                maxBet.text = playerList[i]._betManager.maxBetAvailable.ToString();
            }
        }
        _gameController.myPlayer.GetComponent<PlayerBetManager>().canBet = true;
        BetEnableBtn.SetActive(false);
        BetSlider.SetActive(true);
    }

    public void BetNow()
    {
        BetSlider.SetActive(false);
        CardsPanel.SetActive(true);
        myCardHolder.GetComponent<Image>().raycastTarget = true;
        _gameController.myPlayer.GetComponent<PlayerBetManager>().betPlaced = true;
        pv.RPC("BetForAll", RpcTarget.AllBuffered, null);
    }

    [PunRPC]
    public void BetForAll()
    {
        StartCoroutine(BettingForAll());
    }

    IEnumerator BettingForAll()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            yield return new WaitForSeconds(.1f);

            if (playerList[i]._betManager.betPlaced && playerList[i]._betManager.canBet)
            {
                if (playerList[i].pv.IsMine)
                {
                    playerList[i]._betManager.maxBetAvailable -= (int)BetSlider.GetComponentInChildren<Slider>().value;
                    maxBet.text = playerList[i]._betManager.maxBetAvailable.ToString();
                    playerList[i]._betManager.canBet = false;
                    AvailableBet[i].text = maxBet.text;
                }
            }
            else
            {
                AvailableBet[i].text = playerList[i]._betManager.maxBetAvailable.ToString();
            }
        }
    }

    public void AssigBetValue(float val)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].pv.IsMine)
            {
                playerList[i]._betManager.placedBet = (int)val;
                currentBet.text = ((int)val).ToString();
            }
        }

    }
}


