using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class WagesManager : MonoBehaviourPunCallbacks,IPunObservable
{
    public Controller controller;
    public PlayerObj Obj;
    public ItemDropHandler Drop;

    public GameObject Bet_btn,Final_BetBtn;
    public Slider _Betslider;
    public int _MaxBetValue=10;
    public int xMin = 0, xMax=10;

    // Health
    public GameObject[] _HealthLoader;
    public int _CurrentPlayerBet;   
    public bool _HealthVal;
    public int _Health;
    public int RemaingBet;
    //
    public int _opponentBetted;
    public int OppRemainBet;
    int bet;
    private void Awake() {
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
    }
    void Start()
    {         
        _Betslider.GetComponent<Slider>().maxValue = _MaxBetValue;
        Final_BetBtn.SetActive(false);
        _Betslider.gameObject.SetActive(false);
    }
    private void Update() {
        OpponentValues();
        opponentRemainingBet();
        WaitCall();
    }
    void  WaitCall() {
        for (int i = 0; i < controller._PlayerList.Count; i++) {
            if (controller._PlayerList[i].GetComponent<PlayerObj>()._IsplayerWin || controller._TempBool) {
                print("show values----");
                _AvailableTokens();
            }
           
            }
    }
    //
    void OpponentValues() {
        for (int i = 0; i < controller._PlayerList.Count; i++) {
            if (!controller._PlayerList[i].GetComponent<PlayerObj>().pv.IsMine)
                {
                _opponentBetted = controller._PlayerList[i].GetComponent<PlayerObj>().currentBet;
                }
        }
     }
    void opponentRemainingBet() { // remaining bet 
        for (int i = 0; i < controller._PlayerList.Count; i++) {
            if (!controller._PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
                if (photonView.IsMine) {
                    OppRemainBet = controller._PlayerList[i].GetComponent<PlayerObj>()._RemainingBet;
                }
           }
           
        }
    }

    //
    public void _ClickbettedBtn() {
        _Betslider.gameObject.SetActive(true);
        Bet_btn.SetActive(false);
        Final_BetBtn.SetActive(true);
    }
    public void _ClickBetSlider() {

        for (int i = 0; i < controller._PlayerList.Count; i++) {
            if (controller._PlayerList[i].GetComponent<PlayerObj>().photonView.IsMine) {
                controller._PlayerList[i].GetComponent<PlayerObj>().currentBet = (int)_Betslider.GetComponent<Slider>().value;
                print("currentBet------"+ controller._PlayerList[i].GetComponent<PlayerObj>().currentBet);
                 bet = controller._PlayerList[i].GetComponent<PlayerObj>().currentBet;
            }
        }
    }   
    // Final Click bet
    public void _FinalBetted_Fun() {

        _CurrentPlayerBet = bet;
        print("_BetValue-----" + _CurrentPlayerBet);
        Obj._placedBet = true;
        controller._IsBetActive = true;
        Final_BetBtn.SetActive(false);
        _Betslider.gameObject.SetActive(false);
    }
    void _AvailableTokens() {
        print("token-------");
        for (int i = 0; i < controller._PlayerList.Count; i++) {
            if (controller._PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
                // RemaingBet = _MaxBetValue - _CurrentPlayerBet;               
                // controller.AvailableToken[i].text = RemaingBet.ToString();
                controller._PlayerList[i].GetComponent<PlayerObj>()._RemainingBet = _MaxBetValue - _CurrentPlayerBet;
                controller.AvailableToken[i].text = controller._PlayerList[i].GetComponent<PlayerObj>()._RemainingBet.ToString();
            }
            else {
                controller.AvailableToken[i].text = OppRemainBet.ToString();
            }

        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(_CurrentPlayerBet);
          // stream.SendNext(RemaingBet);
        }
        else if (stream.IsReading) {
            _CurrentPlayerBet = (int)stream.ReceiveNext();
            //RemaingBet = (int)stream.ReceiveNext();
        }
    }
}
