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

    public GameObject BetDetails;
    public GameObject Bet_btn,Final_BetBtn;
    public Slider _Betslider;
    public int _MaxBetValue=10;
    public int xMin = 0, xMax=10;

    // Health
    public GameObject[] _HealthLoader;
    public int _CurrentPlayerBet;   
    public bool _HealthVal;
    public int _Health;
    //
    public int _opponentBetted;
    public int OppRemainBet;
    int bet;
    public TextMeshProUGUI _SliderTxt;
    public Transform Parent;
    public bool _TokenBool;
    private void Awake() {
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
    }
    void Start()
    {         
        _Betslider.GetComponent<Slider>().maxValue = _MaxBetValue;
        BetDetails.SetActive(false);
        
    }
    private void Update() {
        if (!_TokenBool) {
            OpponentValues();
            opponentRemainingBet();
        }
        if (!_TokenBool) {
            for (int i = 0; i < controller._PlayerList.Count; i++) {
                controller.AvailableToken[i].text = _MaxBetValue.ToString();
            }
        }
        for (int i = 0; i < controller._PlayerList.Count; i++) {
            if (controller._PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
                //_Betslider.GetComponent<Slider>().value = Obj._RemainingBet;
                _Betslider.GetComponent<Slider>().value = Mathf.Clamp(_Betslider.GetComponent<Slider>().value, 0, controller._PlayerList[i].GetComponent<PlayerObj>()._RemainingBet);
            }
        }
        UpdateBet();
    }
    public void UpdateBet() 
    {
        for (int i = 0; i < controller.AvailableToken.Length; i++) 
        {
            if (controller._PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
                controller.AvailableToken[0].text = controller._PlayerList[i].GetComponent<PlayerObj>()._RemainingBet.ToString();
            }
            else {
                controller.AvailableToken[1].text = controller._PlayerList[i].GetComponent<PlayerObj>()._RemainingBet.ToString();
            }
            
        }
    }
    //
    void OpponentValues() {
        for (int i = 0; i < controller._PlayerList.Count; i++) {
            if (!controller._PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
                _opponentBetted = controller._PlayerList[i].GetComponent<PlayerObj>().currentBet;
            }
        }
    }
    void opponentRemainingBet() { // remaining bet 
        for (int i = 0; i < controller._PlayerList.Count; i++) {
            if (!controller._PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
                //if (photonView.IsMine) {
                OppRemainBet = controller._PlayerList[i].GetComponent<PlayerObj>()._RemainingBet;
                //}
            }


        }
    }

    //
    public void _ClickbettedBtn() {
        BetDetails.SetActive(true);
        Bet_btn.SetActive(false);
       
    }
    public void _ClickBetSlider() {

        for (int i = 0; i < controller._PlayerList.Count; i++) {
            if (controller._PlayerList[i].GetComponent<PlayerObj>().photonView.IsMine) {
                bet = (int)_Betslider.GetComponent<Slider>().value;              
                 //bet = controller._PlayerList[i].GetComponent<PlayerObj>().currentBet;
                _SliderTxt.transform.parent = Parent; // display slider text
                _SliderTxt.text = bet.ToString();
            }
        }
    }
    
    // Final Click bet
    public void _FinalBetted_Fun() {
        controller.CardVisible.SetActive(true);
        //_CurrentPlayerBet = bet;
       // print("_BetValue-----" + _CurrentPlayerBet);
        Obj._placedBet = true;
        controller._IsBetActive = true;
        BetDetails.SetActive(false);
        for (int i = 0; i < controller._PlayerList.Count; i++) {
            if (!controller._PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {

            }
            else {          
                    controller._CircletxtDisplay[0].SetActive(true);
                    controller._PlaceCardTxt[0].GetComponent<TextMeshProUGUI>().text = bet.ToString();
                    controller._PlayerList[i].GetComponent<PlayerObj>().currentBet = bet;
                }
        }
        _AvailableTokens();
    }
    void _AvailableTokens() {
        _TokenBool = true;
        for (int i = 0; i < controller._PlayerList.Count; i++) {         
                if (controller._PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
                    controller._PlayerList[i].GetComponent<PlayerObj>()._RemainingBet -= bet;
                controller.AvailableToken[i].text = controller._PlayerList[i].GetComponent<PlayerObj>()._RemainingBet.ToString();
            }
                else {
                controller.AvailableToken[i].text = OppRemainBet.ToString();
            }
            // print("token-------");

            //RevealCardAndBet();
        }

    }

    //void RevealCardAndBet() {
    //    for (int i = 0; i < controller._PlayerList.Count; i++) {
    //        if (controller._PlayerList[i].GetComponent<PlayerObj>().pv.IsMine) {
    //            controller._PlaceCardTxt[0].text = controller._PlayerList[i].GetComponent<PlayerObj>().currentBet.ToString();
    //        }
    //        else {
    //            controller._PlaceCardTxt[1].text = controller._PlayerList[i].GetComponent<PlayerObj>().currentBet.ToString();
    //        }
    //    }
    //}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(_CurrentPlayerBet);
            stream.SendNext(_Health);
            stream.SendNext(OppRemainBet);            
        }
        else if (stream.IsReading) {
            _CurrentPlayerBet = (int)stream.ReceiveNext();
            _Health = (int)stream.ReceiveNext();
            OppRemainBet = (int)stream.ReceiveNext();
        }
    }
}
