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
    public PhotonView pv;
    public PlayerObj Obj;
    public WagesManager wages;
   // public ItemDragHandler IDragHandler;

    //buttons
    public GameObject _ReloadBtn, _CloseBtn;
    [Header("5")]
    public List<GameObject> _PlayerList = new List<GameObject>();
    public List<GameObject> _PlaceCardList = new List<GameObject>();
   
    //Timer
    public TextMeshProUGUI _Visual_txt;
    public float _Timer=30;
    public bool _StartTimer;

    // Card Details
    public Sprite[] _PlaceCardSprite;
    public GameObject[] _PlacedCardPos, PlayerOutLine,_CircletxtDisplay;
    public GameObject[] _cardItems;
    public TextMeshProUGUI[] _PlaceCardTxt;
    //Player
    public GameObject[] _PlayerPos;
    public Sprite[] _PlayerSprite;
    public int _CardCnt;
     public bool _TempBool;

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


    public void ReloadApp() {       
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public void CloseApp() {
        Application.Quit();
    }

    private void Start() {
        wages = GameObject.FindGameObjectWithTag("Wages").GetComponent<WagesManager>();
    }
    private void Update() {
        if (!_TempBool) {
            _TimerCall();
        }
        if (!_TempBool) {
            StartCoroutine("WaitTime");
        }
        //
        StartCoroutine("ShowCard");
        
    }
    IEnumerator ShowCard() {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < _PlayerList.Count; i++) {
            if (_PlayerList[i].GetComponent<PlayerObj>()._IsplayerWin || _TempBool) {
                AfterBet();
            }
        }

    }
        IEnumerator WaitTime() {
        yield return new WaitForSeconds(2f);
        _ResolutionUpdate();
    }
    void _ResolutionUpdate() {
        if (_PlayerList.Count == _PlaceCardList.Count) {
            for (int i = 0; i < _PlaceCardList.Count; i++) {
                for (int j = i + 1; j < _PlaceCardList.Count; j++) {
                    for (int a = 0; a < _PlayerList.Count; a++) {//health cal
                        if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 0 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 0) {
                            print("1----1----");
                            _TempBool = true;
                            _Visual_txt.text = "Same Card........";
                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 1 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 1) {
                            print("2----");
                            _TempBool = true;
                            _Visual_txt.text = "Same Card........";

                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 2 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 2) {
                            print("3----");
                            _TempBool = true;
                            _Visual_txt.text = "Same Card........";

                        }

                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 0 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 1) {
                            print("attack beats");
                            _TempBool = true;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _Visual_txt.text = _PlaceCardList[j].GetComponent<PlayerObj>().avatarName + "  Is Won........";
                            if (_PlaceCardList[i].GetComponent<PlayerObj>().pv.IsMine) {
                                wages._Health = _PlaceCardList[j].GetComponent<PlayerObj>().currentBet + 1;
                                //_PlayerList[a].GetComponent<WagesManager>()._Health = _PlayerList[a].GetComponent<WagesManager>()._BetValue + 1;
                                HealthLoader[i].GetComponent<Image>().fillAmount = (float)(_MaxHealth - wages._Health) / _MaxHealth;
                                print("Health-------" + wages._Health + "_MaxHealth---" + _MaxHealth);
                            }
                            

                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 1 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 0) {
                            print("attack beats");
                            _TempBool = true;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _Visual_txt.text = _PlaceCardList[i].GetComponent<PlayerObj>().avatarName + "Is Won........";
                            if (_PlaceCardList[j].GetComponent<PlayerObj>().pv.IsMine) {
                                wages._Health = _PlaceCardList[i].GetComponent<PlayerObj>().currentBet + 1;
                                // _PlayerList[a].GetComponent<WagesManager>()._Health = _PlayerList[a].GetComponent<WagesManager>()._BetValue + 1;
                                HealthLoader[j].GetComponent<Image>().fillAmount = (float)(_MaxHealth - wages._Health) / _MaxHealth;
                                print("Health-------" + wages._Health + "_MaxHealth---" + _MaxHealth);
                            }
                            
                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 1 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 2) {
                            print("throw beats");
                            _TempBool = true;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _Visual_txt.text = _PlaceCardList[j].GetComponent<PlayerObj>().avatarName + "  Is Won........";
                            if (_PlaceCardList[i].GetComponent<PlayerObj>().pv.IsMine) {
                                wages._Health = _PlaceCardList[j].GetComponent<PlayerObj>().currentBet + 1;
                                HealthLoader[i].GetComponent<Image>().fillAmount = (float)(_MaxHealth - wages._Health) / _MaxHealth;
                            }
                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 2 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 1) {
                            print("throw beats");
                            _TempBool = true;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _Visual_txt.text = _PlaceCardList[i].GetComponent<PlayerObj>().avatarName + "Is Won........";
                            if (_PlaceCardList[j].GetComponent<PlayerObj>().pv.IsMine) {
                                wages._Health = _PlaceCardList[j].GetComponent<PlayerObj>().currentBet + 1;
                                HealthLoader[j].GetComponent<Image>().fillAmount = (float)(_MaxHealth - wages._Health) / _MaxHealth;
                            }
                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 0 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 2) {
                            print("attack beats");
                            _TempBool = true;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _Visual_txt.text = _PlaceCardList[i].GetComponent<PlayerObj>().avatarName + "  Is Won........";
                            if (_PlaceCardList[j].GetComponent<PlayerObj>().pv.IsMine) {
                                wages._Health = _PlaceCardList[j].GetComponent<PlayerObj>().currentBet + 1;
                                HealthLoader[j].GetComponent<Image>().fillAmount = (float)(_MaxHealth - wages._Health) / _MaxHealth;
                            }
                        }
                        else if (_PlaceCardList[i].GetComponent<PlayerObj>().CardId == 2 && _PlaceCardList[j].GetComponent<PlayerObj>().CardId == 0) {
                            print("attack beats");
                            _TempBool = true;
                            _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = false;
                            _PlaceCardList[j].GetComponent<PlayerObj>()._IsplayerWin = true;
                            _Visual_txt.text = _PlaceCardList[j].GetComponent<PlayerObj>().avatarName + "Is Won........";
                            if (_PlaceCardList[i].GetComponent<PlayerObj>().pv.IsMine) {
                                wages._Health = _PlaceCardList[j].GetComponent<PlayerObj>().currentBet + 1;
                                HealthLoader[i].GetComponent<Image>().fillAmount = (float)(_MaxHealth - wages._Health) / _MaxHealth;
                            }
                        }
                    }
                }

            }
            StartCoroutine("Reset");
           
            print("clear--------");
        }
    }
    IEnumerator Reset() {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < _PlaceCardList.Count; i++) {
            for (int j = 0; j < _PlaceCardList.Count; j++) {
                _PlaceCardList[i].GetComponent<PlayerObj>()._PlacedCard = false;
                _PlaceCardList[i].GetComponent<PlayerObj>().CardId = 99;
                _PlaceCardList[i].GetComponent<PlayerObj>()._IsplayerWin = false;
               _TempBool = false;
                _PlaceCardList[i].GetComponent<PlayerObj>().enabled = false;
                PlayerOutLine[i].GetComponent<Image>().sprite = OutlineCard;//sprite
                _CircletxtDisplay[i].SetActive(false);
                _IsBetActive = false;
                _StartTimer = true;
                _Visual_txt.text = "Place The Card........";
               wages. Final_BetBtn.SetActive(false);
               wages._Betslider.gameObject.SetActive(false);
               wages.Bet_btn.SetActive(true);
            }
        }

        _PlaceCardList.Clear();
    }
    void _TimerCall() {     
           // print("false---");
            if (!_StartTimer) {
                _Timer -= Time.deltaTime;
                _Visual_txt.text = (int)_Timer + " Seconds Left";
                if (_Timer <= 0) {
                    print("--" + _Timer);
                    _StartTimer = true;
                    _Visual_txt.text = "Time Up";
                }

            }     
    }
    //card Placing
    public void _CardPlacing() {
        photonView.RPC("Placecard", RpcTarget.AllBuffered, null);
    }

    [PunRPC]
    public void Placecard() {
        StartCoroutine("_PlaceCard",0.1f);
       
    }
    IEnumerator _PlaceCard() {
        for (int i = 0; i < _PlayerList.Count; i++) {
            yield return new WaitForSeconds(0.1f);

            if (_PlayerList[i].GetComponent<PlayerObj>()._PlacedCard && _IsBetActive) {

                if (_PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
                    PlayerOutLine[i].GetComponent<Image>().sprite = _PlaceCardSprite[_PlayerList[i].GetComponent<PlayerObj>().CardId];                  
                }
                else {
                    PlayerOutLine[i].GetComponent<Image>().sprite = Ready_Card;
                }
                if (!_PlaceCardList.Contains(_PlayerList[i])) {
                    _PlaceCardList.Add(_PlayerList[i]);
                }
            }
        }       

    }  
    void AfterBet() {
        for (int i = 0; i < _PlayerList.Count; i++) {
            if (!_PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
                print("not mine----");
                PlayerOutLine[i].GetComponent<Image>().sprite = _PlaceCardSprite[_PlayerList[i].GetComponent<PlayerObj>().CardId];
                _CircletxtDisplay[i].SetActive(true);
                _PlaceCardTxt[i].GetComponent<TextMeshProUGUI>().text = wages._opponentBetted.ToString();
            }
            else {
                print(" mine--5--");
                _CircletxtDisplay[i].SetActive(true);
                _PlaceCardTxt[i].GetComponent<TextMeshProUGUI>().text = wages._CurrentPlayerBet.ToString();
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {          
            stream.SendNext(_CardCnt);
            stream.SendNext(_IsBetActive);
            stream.SendNext(_TempBool);
            //stream.SendNext(_Health);
        }
        else if (stream.IsReading) {         
            _CardCnt = (int)stream.ReceiveNext();
            _IsBetActive = (bool)stream.ReceiveNext();
            _TempBool = (bool)stream.ReceiveNext();
            //_Health = (int)stream.ReceiveNext();
        }
    }
}
