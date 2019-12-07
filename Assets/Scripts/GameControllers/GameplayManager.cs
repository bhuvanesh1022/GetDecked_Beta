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
    public List<TextMeshProUGUI> AvailableBet = new List<TextMeshProUGUI>();
    public List<Image> HealthBar = new List<Image>();
    public List<Image> Specials = new List<Image>();
    public List<Sprite>cards = new List<Sprite>();
    public DropManager myCardHolder;
    public Image myCard;
    public Image opponentCard;
    public Sprite ReadyCard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
