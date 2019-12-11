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
    public PlayerObj Obj;
    public WagesManager wages;
    public ItemDragHandler IDragHandler;

    //buttons
    public GameObject _ReloadBtn, _CloseBtn;
    [Header("5")]
    public List<GameObject> _PlayerList = new List<GameObject>();
   // public List<GameObject> _PlaceCardList = new List<GameObject>();
   //Timer
    public TextMeshProUGUI _Visual_txt;
    public float _Timer=30;
    public bool _StartTimer;
    public float Speed;

    // Card Details
    public Sprite[] _PlaceCardSprite;
    public GameObject[] _PlacedCardPos, PlayerOutLine,_CircletxtDisplay;
    public GameObject[] _cardItems;
    public TextMeshProUGUI[] _PlaceCardTxt;
    //Player
    public GameObject[] _PlayerPos;
    public Sprite[] _PlayerSprite;
    public int _CardCnt;
   // public bool _ISPlacedCard;
    [Header("5")]
    public int _MaxHealth=10;
    // bool
    public bool _IsBetActive;
    //health
    public int _Health = 10;
    public Sprite Ready_Card;
    public int _OpponentBetted;
    public bool _ValueChangedToThisPlayer;


    public void ReloadApp() {       
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(1);
    }

    public void CloseApp() {
        Application.Quit();
    }

    private void Start() {
        wages = GameObject.FindGameObjectWithTag("Wages").GetComponent<WagesManager>();
    }
    private void Update() {
        if (!_StartTimer) {           
            _Timer -=   Time.deltaTime;
            _Visual_txt.text= (int)_Timer+" Seconds Left";
            if (_Timer >= 0) {
                print("--"+ _Timer);
                _StartTimer = true;
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
        print("i enumerator------");
        yield return new WaitForSeconds(0.1f);
        if (_IsBetActive == true) {
            for (int i = 0; i < _PlacedCardPos.Length; i++) {
                if (_PlayerList[i].GetComponent<PlayerObj>()._PlayerCardVal) {
                    if (_PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
                        print("placecard------");

                        PlayerOutLine[0].SetActive(true);
                        _CircletxtDisplay[0].SetActive(true);
                         PlayerOutLine[0].GetComponent<Image>().sprite = _PlaceCardSprite[_CardCnt];
                        _PlaceCardTxt[0].GetComponent<TextMeshProUGUI>().text = wages._FinalBet.ToString();
                    }
                    else {
                        print("else------");

                        //PlayerOutLine[1].SetActive(true);
                        //_CircletxtDisplay[1].SetActive(true);
                        //PlayerOutLine[1].GetComponent<Image>().sprite = _PlaceCardSprite[_CardCnt];
                        //_PlaceCardTxt[1].GetComponent<TextMeshProUGUI>().text = wages._FinalBet.ToString();

                        PlayerOutLine[1].SetActive(true);
                        _CircletxtDisplay[1].SetActive(true);
                        PlayerOutLine[1].GetComponent<Image>().sprite = _PlaceCardSprite[_CardCnt];
                    }
                }

                //if (IDragHandler.Cnt == 0 && IDragHandler.Cnt == 1) {
                if (_cardItems[0] && _cardItems[1] && photonView.IsMine) {
                    print("Defend beats");
                    _Visual_txt.GetComponent<TextMeshProUGUI>().text = Obj.avatarName + "Win";
                    _Visual_txt.GetComponent<TextMeshProUGUI>().text = _OpponentBetted + 1.ToString();
                    _ValueChangedToThisPlayer = true;

                }
              
                else if (_cardItems[0] && _cardItems[2] && photonView.IsMine) {
                    print("Attack beats");
                    _Visual_txt.GetComponent<TextMeshProUGUI>().text = Obj.avatarName + "Win";
                    _Visual_txt.GetComponent<TextMeshProUGUI>().text = _OpponentBetted + 1.ToString();
                    _ValueChangedToThisPlayer = true;
                }
               
                else if (_cardItems[1] && _cardItems[2] && photonView.IsMine) {
                    print("throw beats");
                    _Visual_txt.GetComponent<TextMeshProUGUI>().text = Obj.avatarName + "Win";
                    _Visual_txt.GetComponent<TextMeshProUGUI>().text = _OpponentBetted + 1.ToString();
                    _ValueChangedToThisPlayer = true;
                }
               

            }
            _IsBetActive = false;
            _ValueChangedToThisPlayer = false;
        }
       
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {          
            stream.SendNext(_CardCnt);
            stream.SendNext(_IsBetActive);
        }
        else if (stream.IsReading) {         
            _CardCnt = (int)stream.ReceiveNext();
            _IsBetActive = (bool)stream.ReceiveNext();
        }
    }
}
