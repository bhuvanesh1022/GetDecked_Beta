using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class WagesManager : MonoBehaviour
{
    public Controller controller;
    public GameObject Bet_btn,Final_Bet;
    public Slider _Betslider;
    public int _BetValue,_MaxBetValue=10;
    public int xMin = 0, xMax=10;

    // Health
    public GameObject _HealthBar,_HealthLoad;
    public GameObject[] _HealthLoader;
    public int _FinalBet;
    int SetScale = 1;
    public bool _HealthVal;

    private void Awake() {
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
    }
    void Start()
    {
         
        _Betslider.GetComponent<Slider>().maxValue = _MaxBetValue;
        Final_Bet.SetActive(false);
        _Betslider.gameObject.SetActive(false);
    }
    //
    public void _ClickbettedBtn() {
        _Betslider.gameObject.SetActive(true);
        Bet_btn.SetActive(false);
        Final_Bet.SetActive(true);
    }
    public void _ClickBetSlider() {
        
        _BetValue = (int)_Betslider.GetComponent<Slider>().value;
       
    }
    // Final Click bet
    public void _FinalBetted_Fun() {

        _FinalBet = Mathf.Clamp(_BetValue, xMin, xMax -_BetValue);

        print("_BetValue--"  + _FinalBet);
        controller._IsBetActive = true;
        Final_Bet.SetActive(false);
        _Betslider.gameObject.SetActive(false);
    }

    // Health Loader
    public void _HealthLoading() {
        for (int i=0;i<_HealthLoader.Length;i++) {
            _HealthLoader[i].SetActive(true);
           
        }
    }

}
