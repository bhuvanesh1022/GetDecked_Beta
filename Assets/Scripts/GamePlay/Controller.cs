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
    //buttons
    public GameObject _ReloadBtn, _CloseBtn;
    [Header("5")]
    public List<GameObject> _PlayerList = new List<GameObject>();
    public List<GameObject> _PlaceCardList = new List<GameObject>();
    public TextMeshProUGUI _Visual_txt;

    // Card Details
    public Sprite[] _PlaceCardSprite;
    public GameObject[] _PlacedCardPos, PlayerOutLine;
    public GameObject[] _cardItems;
    public TextMeshProUGUI[] _PlaceCardTxt;
    //Player
    public GameObject[] _PlayerPos;
    public Sprite[] _PlayerSprite;
    public int _CardCnt;
    public bool _ISPlacedCard;
    [Header("5")]
    public int _MaxHealth=10;
    // bool
    public bool _IsBetActive;
    //health
    public int _Health = 10;


    public void ReloadApp() {       
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(1);
    }

    public void CloseApp() {
        Application.Quit();
    }

    private void Start() {
       
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(_ISPlacedCard);
            stream.SendNext(_CardCnt);
            stream.SendNext(_IsBetActive);
        }
        else if (stream.IsReading) {
            _ISPlacedCard = (bool)stream.ReceiveNext();
            _CardCnt = (int)stream.ReceiveNext();
            _IsBetActive = (bool)stream.ReceiveNext();
        }
    }
}
