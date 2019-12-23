using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class SpecialManager : MonoBehaviourPunCallbacks
{
    public Controller controller;
    public GameObject SpecialCard;

    float bet;

    private void Awake() {
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
    }
    public void WintiesSpecial() {
        
        for (int i = 0; i < controller._PlaceCardList.Count; i++) {
            if (controller._PlaceCardList[i].GetComponent<PlayerObj>()._SpecialCardActive) {
                bet = controller._PlaceCardList[i].GetComponent<PlayerObj>().currentBet + 1;
            }   
        }

        for (int i = 0; i < controller._PlaceCardList.Count; i++) {
            if (!controller._PlaceCardList[i].GetComponent<PlayerObj>()._SpecialCardActive) {
                controller._PlaceCardList[i].GetComponent<PlayerObj>().currentHealth -= bet;
            }
        }
            
    }
    public void LifestealSpecial() {
        for (int i = 0; i < controller._PlaceCardList.Count; i++) {
            if (controller._PlaceCardList[i].GetComponent<PlayerObj>()._SpecialCardActive) {
                bet = controller._PlaceCardList[i].GetComponent<PlayerObj>().currentBet + 1;
                controller._PlaceCardList[i].GetComponent<PlayerObj>().currentHealth += bet;
                controller._PlaceCardList[i].GetComponent<PlayerObj>().currentHealth = Mathf.Clamp(controller._PlaceCardList[i].GetComponent<PlayerObj>().currentHealth, 0, controller._MaxHealth);
            }
        }
    }


    // 
    public void SpecialCardBtn_click() {
        controller.Obj._SpecialCardActive = true;
        SpecialCard.SetActive(false);
       // controller.Obj.GetComponent<CanvasGroup>().interactable = false;
        print("-------special");
    }
    }
