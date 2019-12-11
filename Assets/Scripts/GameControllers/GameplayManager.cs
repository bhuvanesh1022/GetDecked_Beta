using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public List<PlayerController> playerList = new List<PlayerController>();
    public List<GameObject> playerPanels = new List<GameObject>();
    public List<TextMeshProUGUI> playerNames = new List<TextMeshProUGUI>();

    public int MaxBet;
    public List<TextMeshProUGUI> AvailableBet = new List<TextMeshProUGUI>();
    public List<Image> HealthBar = new List<Image>();
    public List<Image> Specials = new List<Image>();

    public List<Sprite>cards = new List<Sprite>();
    public DropManager myCardHolder;
    public List<Image> playedCards = new List<Image>();
    public Sprite ReadyCard;

    public List<PlayerController> playersPlayed = new List<PlayerController>();
}
