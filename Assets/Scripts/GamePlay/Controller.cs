using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Controller : MonoBehaviourPunCallbacks,IPunObservable
{
    public enum SpecialClass { WinTies, LifeSteal };
    public SpecialClass activeSpecial;

    public SpecialManager SPmanager;
    public PhotonView pv;
    public PlayerObj Obj;
    public WagesManager wages;
   // public ItemDragHandler IDragHandler;

    //buttons
    public GameObject _ReloadBtn, _CloseBtn;
    [Header("5")]
    public List<GameObject> _PlayerList = new List<GameObject>();
    public List<GameObject> _PlaceCardList = new List<GameObject>();
    public List<Sprite> _SpecialList = new List<Sprite>();
    //Timer
    public TextMeshProUGUI _Visual_txt;
    public float _Timer=30;
    public bool _StartTimer;

    // Card Details
    public Sprite[] _PlaceCardSprite;
    public GameObject[] _PlacedCardHolder, PlayerOutLine,_CircletxtDisplay;
    public GameObject[] _cardItems;
    public TextMeshProUGUI[] _PlaceCardTxt;
    //Player
    public GameObject[] _PlayerPos;
    public Sprite[] _PlayerSprite;
    public int _CardCnt;
     public bool _TempBool,_FinishTurn;
    public GameObject CardVisible;

    //health
    [Header("Health details")]
    public int _MaxHealth=10;  
    public bool _IsBetActive;
  
    public int _TotHealth = 10;
    public Sprite Ready_Card;
    public Sprite OutlineCard;
    public int _OpponentBetted;
    public GameObject[] HealthLoader;
    public TextMeshProUGUI[] AvailableToken;
    public TextMeshProUGUI[] HealthBarText;

    [Header("Special details")]
    public GameObject[] _specialIcons;
    int SpecialInt;
    public bool _gameFinished;
    public bool resetBet;
    [Header("Fighters")]
    public GameObject[] Fighters;

    public int _Maxspecialcount;
    public bool differentSpl;
    public bool assignSpl;
    public bool _ResolutionVal;
     

    public void ReloadApp() {       
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public void CloseApp() {
        Application.Quit();
    }

    private void Start() {
        SPmanager = GameObject.FindGameObjectWithTag("Special").GetComponent<SpecialManager>();
        wages = GameObject.FindGameObjectWithTag("Wages").GetComponent<WagesManager>();
        CardVisible.SetActive(false);
        _Visual_txt.text = "Place the bet and card.......";
       
        for (int i = 0; i < _PlayerList.Count; i++) {
            _PlayerList[i].GetComponent<PlayerObj>().currentHealth = _MaxHealth;
        }
        assignSpl = true;
        }
    private void Update() {
        //if (!_TempBool) {
        //    _TimerCall();
        //}

        if (_ResolutionVal) {
            _ResolutionVal = false;
            print("rpc call------");
            StartCoroutine("_ResolutionUpdate");
        }

        for (int i = 0; i < _PlayerList.Count; i++) {
            _PlayerList[i].GetComponent<PlayerObj>().healthbar.GetComponent<Image>().fillAmount = _PlayerList[i].GetComponent<PlayerObj>().currentHealth / _MaxHealth;
            _PlayerList[i].GetComponent<PlayerObj>()._HealthTxt.GetComponent<TextMeshProUGUI>().text = _PlayerList[i].GetComponent<PlayerObj>().currentHealth.ToString();
        }
        for (int i = 0; i < _PlayerList.Count; i++) {
            if (_PlayerList[i].GetComponent<PlayerObj>().healthbar.GetComponent<Image>().fillAmount <= 0 && !_gameFinished) {
                print("player name"+ _PlayerList[i].GetComponent<PlayerObj>().avatarName);
                photonView.RPC("GameFinish", RpcTarget.AllBuffered, null);
            }
        }
        if (assignSpl) {
            for (int i = 0; i < _PlayerList.Count; i++) {
                int Rand = Random.Range(0, _Maxspecialcount);
                _PlayerList[i].GetComponent<PlayerObj>()._AvailableSpecial = Rand;               
                assignSpl = false;
            }
        }       
        if (_PlayerList[1].GetComponent<PlayerObj>()._AvailableSpecial == _PlayerList[0].GetComponent<PlayerObj>()._AvailableSpecial) {
            int rand = Random.Range(0, _Maxspecialcount);
            _PlayerList[1].GetComponent<PlayerObj>()._AvailableSpecial = rand;           
        }
        else {
            SPmanager.SpecialCard.GetComponent<Image>().sprite = _SpecialList[Obj._AvailableSpecial];          
        }

    }
    [PunRPC]
    public void GameFinish() {
        _gameFinished = true;
        StartCoroutine("FinishGame");
    }

    IEnumerator FinishGame() {
        yield return new WaitForSeconds(2f);
        wages.Bet_btn.SetActive(false);
        _Visual_txt.text = " Game Finished !!!";
        CardVisible.SetActive(false);
    }
    //void _TimerCall() {     
    //        if (!_StartTimer) {
    //            _Timer -= Time.deltaTime;
    //            _Visual_txt.text = (int)_Timer + " Seconds Left";
    //            if (_Timer <= 0) {
    //                print("--" + _Timer);
    //                _StartTimer = true;
    //                _Visual_txt.text = "Time Up";
    //            StartCoroutine("Reset", 2f);
    //        }

    //        }     
    //}

    //card Placing
    public void _CardPlacing() {
        photonView.RPC("Placecard", RpcTarget.AllBuffered, null);
    }

    [PunRPC]
    public void Placecard() {
        StartCoroutine("_PlaceCard",0.1f);
       
    }
    IEnumerator _PlaceCard() {
        //for (int i = 0; i < _PlayerList.Count; i++) {
        //    yield return new WaitForSeconds(0.2f);
        //    if (_PlayerList[i].GetComponent<PlayerObj>()._PlacedCard && _PlayerList[i].GetComponent<PlayerObj>()._placedBet) {
        //        if (_PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
        //            PlayerOutLine[0].SetActive(false);
        //            _PlacedCardHolder[0].GetComponent<Image>().enabled = true;
        //            _PlacedCardHolder[0].GetComponent<Image>().sprite = _PlaceCardSprite[_PlayerList[i].GetComponent<PlayerObj>().CardId];
        //        }
        //        else {
        //            PlayerOutLine[1].SetActive(false);
        //            _PlacedCardHolder[1].GetComponent<Image>().enabled = true;
        //            _PlacedCardHolder[1].GetComponent<Image>().sprite = Ready_Card;
        //            _CircletxtDisplay[1].SetActive(false);
        //        }

        //        if (!_PlaceCardList.Contains(_PlayerList[i])) {
        //            _PlaceCardList.Add(_PlayerList[i]);
        //        }

        //    }
        //}

        yield return new WaitForSeconds(0.2f);
        if (_PlayerList[0].GetComponent<PlayerObj>().pv.IsMine) {
            if (_PlayerList[0].GetComponent<PlayerObj>()._PlacedCard && _PlayerList[0].GetComponent<PlayerObj>()._placedBet) {
                PlayerOutLine[0].SetActive(false);
                 _PlacedCardHolder[0].GetComponent<Image>().enabled = true;
                 _PlacedCardHolder[0].GetComponent<Image>().sprite = _PlaceCardSprite[_PlayerList[0].GetComponent<PlayerObj>().CardId];
            }
        }
       else {
            if (_PlayerList[0].GetComponent<PlayerObj>()._PlacedCard && _PlayerList[0].GetComponent<PlayerObj>()._placedBet) {
                PlayerOutLine[1].SetActive(false);
                _PlacedCardHolder[1].GetComponent<Image>().enabled = true;
                _PlacedCardHolder[1].GetComponent<Image>().sprite = Ready_Card;
                _CircletxtDisplay[1].SetActive(false);
            }
        }

        if (_PlayerList[1].GetComponent<PlayerObj>().pv.IsMine) {
            if (_PlayerList[1].GetComponent<PlayerObj>()._PlacedCard && _PlayerList[1].GetComponent<PlayerObj>()._placedBet) {
                PlayerOutLine[0].SetActive(false);
                _PlacedCardHolder[0].GetComponent<Image>().enabled = true;
                _PlacedCardHolder[0].GetComponent<Image>().sprite = _PlaceCardSprite[_PlayerList[1].GetComponent<PlayerObj>().CardId];
            }
        }
        else {
            if (_PlayerList[1].GetComponent<PlayerObj>()._PlacedCard && _PlayerList[1].GetComponent<PlayerObj>()._placedBet) {
                PlayerOutLine[1].SetActive(false);
                _PlacedCardHolder[1].GetComponent<Image>().enabled = true;
                _PlacedCardHolder[1].GetComponent<Image>().sprite = Ready_Card;
                _CircletxtDisplay[1].SetActive(false);
            }
        }

        for (int i = 0; i < _PlayerList.Count; i++) {
            if (!_PlaceCardList.Contains(_PlayerList[i]) && _PlayerList[i].GetComponent<PlayerObj>()._PlacedCard) {
            _PlaceCardList.Add(_PlayerList[i]);
            }
         }
        if (_PlayerList.Count == _PlaceCardList.Count) {
            _ResolutionVal = true;
        }
            
        //StartCoroutine("_ResolutionUpdate");

    }

    IEnumerator _ResolutionUpdate() {
        _ResolutionVal = false;
        yield return new WaitForSeconds(1f);
        activeSpecial = SpecialClass.LifeSteal;
        if (_PlayerList.Count == _PlaceCardList.Count) {
            CardVisible.SetActive(false);
            for (int i = 0; i < _PlaceCardList.Count; i++) {
                for (int j = i + 1; j < _PlaceCardList.Count; j++) {
                    
                        if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 0 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 0) {
                            _TempBool = true;
                            _Visual_txt.text = "Same Card........";
                        activeSpecial = SpecialClass.WinTies;
                         AfterBet();                      

                    }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 1 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 1) {
                            _TempBool = true;
                            _Visual_txt.text = "Same Card........";
                        activeSpecial = SpecialClass.WinTies;
                        AfterBet();
                    }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 2 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 2) {
                            _TempBool = true;
                            _Visual_txt.text = "Same Card........";
                        activeSpecial = SpecialClass.WinTies;
                        AfterBet();
                    }

                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 0 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 1) {
                            print("Defend beats");
                            _TempBool = true;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _Visual_txt.text = _PlaceCardList[j].GetComponent<PlayerObj>().avatarName + "  Is Won........";

                        _PlaceCardList[j].GetComponent<PlayerObj>().myFighter.Play("Character one attack");
                        yield return new WaitForSeconds(0.2f);
                        _PlaceCardList[i].GetComponent<PlayerObj>().myFighter.Play("Character spear");
                        _PlaceCardList[i].GetComponent<PlayerObj>().Wage._Health = _PlaceCardList[j].GetComponent<PlayerObj>().currentBet + 1;
                            _PlaceCardList[i].GetComponent<PlayerObj>().updateHealth = true;
                            AfterBet();

                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 1 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 0) {
                            print("Defend beats");
                            _TempBool = true;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _Visual_txt.text = _PlaceCardList[i].GetComponent<PlayerObj>().avatarName + "Is Won........";
                        _PlaceCardList[i].GetComponent<PlayerObj>().myFighter.Play("Character one attack");
                        yield return new WaitForSeconds(0.2f);
                        _PlaceCardList[j].GetComponent<PlayerObj>().myFighter.Play("Character spear");                       
                        _PlaceCardList[j].GetComponent<PlayerObj>().Wage._Health = _PlaceCardList[i].GetComponent<PlayerObj>().currentBet + 1;
                            _PlaceCardList[j].GetComponent<PlayerObj>().updateHealth = true;
                            AfterBet();
                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 1 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 2) {
                            print("throw beats");
                            _TempBool = true;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _Visual_txt.text = _PlaceCardList[j].GetComponent<PlayerObj>().avatarName + "  Is Won........";
                        _PlaceCardList[j].GetComponent<PlayerObj>().myFighter.Play("Character one attack");
                        yield return new WaitForSeconds(0.2f);
                        _PlaceCardList[i].GetComponent<PlayerObj>().myFighter.Play("Character spear");
                        _PlaceCardList[i].GetComponent<PlayerObj>().Wage._Health = _PlaceCardList[j].GetComponent<PlayerObj>().currentBet + 1;
                        _PlaceCardList[i].GetComponent<PlayerObj>().updateHealth = true;                      
                         AfterBet();
                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 2 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 1) {
                            print("throw beats");
                            _TempBool = true;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _Visual_txt.text = _PlaceCardList[i].GetComponent<PlayerObj>().avatarName + "Is Won........";
                        _PlaceCardList[i].GetComponent<PlayerObj>().myFighter.Play("Character one attack");
                        yield return new WaitForSeconds(0.2f);
                        _PlaceCardList[j].GetComponent<PlayerObj>().myFighter.Play("Character spear");
                        _PlaceCardList[j].GetComponent<PlayerObj>().Wage._Health = _PlaceCardList[i].GetComponent<PlayerObj>().currentBet + 1;
                        _PlaceCardList[j].GetComponent<PlayerObj>().updateHealth = true;
                         AfterBet();
                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 0 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 2) {
                            print("attack beats");
                            _TempBool = true;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _Visual_txt.text = _PlaceCardList[i].GetComponent<PlayerObj>().avatarName + "  Is Won........";
                        _PlaceCardList[i].GetComponent<PlayerObj>().myFighter.Play("Character one attack");
                        yield return new WaitForSeconds(0.2f);
                        _PlaceCardList[j].GetComponent<PlayerObj>().myFighter.Play("Character spear");
                        _PlaceCardList[j].GetComponent<PlayerObj>().Wage._Health = _PlaceCardList[i].GetComponent<PlayerObj>().currentBet + 1;
                        _PlaceCardList[j].GetComponent<PlayerObj>().updateHealth = true;
                        AfterBet();
                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 2 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 0) {
                            print("attack beats");
                            _TempBool = true;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _Visual_txt.text = _PlaceCardList[j].GetComponent<PlayerObj>().avatarName + "Is Won........";
                         _PlaceCardList[j].GetComponent<PlayerObj>().myFighter.Play("Character one attack");
                        yield return new WaitForSeconds(0.2f);
                        _PlaceCardList[i].GetComponent<PlayerObj>().myFighter.Play("Character spear");

                        _PlaceCardList[i].GetComponent<PlayerObj>().Wage._Health = _PlaceCardList[j].GetComponent<PlayerObj>().currentBet + 1;
                        _PlaceCardList[i].GetComponent<PlayerObj>().updateHealth = true;
                        AfterBet();

                        }
                    
                }

            }
            CallingSpecial();
            StartCoroutine("Reset", 4f);
           // print("clear--------");
        }
    }
    void CallingSpecial() {
        for (int a = 0; a < _PlaceCardList.Count; a++) {
            if (_PlaceCardList[a].GetComponent<PlayerObj>()._SpecialCardActive) {            
                switch (activeSpecial) {
                    case SpecialClass.WinTies:
                        SPmanager.WintiesSpecial();
                        break;
                    case SpecialClass.LifeSteal:
                        SPmanager.LifestealSpecial();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    void Fighters_ShowFun() {
        print("show fun----");
        for (int i = 0; i < Fighters.Length; i++) {
            Fighters[i].SetActive(true);
        }
    }

    void AfterBet() {
        for (int i = 0; i < _PlayerList.Count; i++) {
            if (!_PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {            
                 PlayerOutLine[1].SetActive(false);
                _PlacedCardHolder[1].GetComponent<Image>().enabled = true;
                _PlacedCardHolder[1].GetComponent<Image>().sprite = _PlaceCardSprite[_PlayerList[i].GetComponent<PlayerObj>().CardId];
                 _CircletxtDisplay[1].SetActive(true);
               _PlaceCardTxt[1].GetComponent<TextMeshProUGUI>().text = _PlayerList[i].GetComponent<PlayerObj>().currentBet.ToString();
            }
           
        }
    }

    IEnumerator Reset() {
        _FinishTurn = true;
        //for (int i = 0; i < Fighters.Length; i++) {
        //    Fighters[i].SetActive(false);
        //    Fighters[i].GetComponentInChildren<Animator>().Play("Character one idle");
        //}
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < _PlaceCardList.Count; i++) {
            //for (int j = 0; j < _PlaceCardList.Count; j++) {
                _PlaceCardList[i].GetComponent<PlayerObj>()._PlacedCard = false;
                _PlaceCardList[i].GetComponent<PlayerObj>().CardId = 99;
                _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = false;
                _PlayerList[i].GetComponent<PlayerObj>()._IsplayerWin = false;
                _PlaceCardList[i].GetComponent<PlayerObj>().canUpdateHealth = true;
                _PlaceCardList[i].GetComponent<PlayerObj>().myFighter.Play("Character one idle");
                _TempBool = false;
                PlayerOutLine[i].SetActive(true);
                PlayerOutLine[i].GetComponent<Image>().sprite = OutlineCard;//sprite
                _PlacedCardHolder[i].GetComponent<Image>().enabled = false;
                _CircletxtDisplay[i].SetActive(false);
                _IsBetActive = false;
               // _StartTimer = true;
                wages.BetDetails.SetActive(false);
                _PlaceCardList[i].GetComponent<PlayerObj>()._SpecialCardActive = false;
            //}
        }
        wages.EnableImg.raycastTarget = false;
        resetBet = _PlayerList[0].GetComponent<PlayerObj>()._RemainingBet <= 0 && _PlayerList[1].GetComponent<PlayerObj>()._RemainingBet <= 0;
        Debug.Log(resetBet);

        yield return new WaitForSeconds(1f);
        _PlaceCardList.Clear();
        if (!_gameFinished) {
            _Visual_txt.text = "Next Turn........";
        }

        yield return new WaitForSeconds(1.5f);
        if (!_gameFinished) {
            wages.Bet_btn.SetActive(true);
            _TempBool = false;
           // _StartTimer = false;
           // _Timer = 30;
        }

        if (!_gameFinished) {
            _Visual_txt.text = "Place The Card........";          
        }

        for (int i = 0; i < _PlayerList.Count; i++) {
            if (resetBet && !_gameFinished) {
                print("Full health--------");
                _PlayerList[i].GetComponent<PlayerObj>()._RemainingBet = wages._MaxBetValue;
            }
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {          
            stream.SendNext(_CardCnt);
            stream.SendNext(_IsBetActive);
            stream.SendNext(_TempBool);
        }
        else if (stream.IsReading) {         
            _CardCnt = (int)stream.ReceiveNext();
            _IsBetActive = (bool)stream.ReceiveNext();
            _TempBool = (bool)stream.ReceiveNext();          
        }
    }
}
