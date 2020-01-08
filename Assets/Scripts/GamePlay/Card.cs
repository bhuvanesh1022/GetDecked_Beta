using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.EventSystems;
public class Card : MonoBehaviourPunCallbacks
{
    public Transform _OuterParent;
    Vector3 _Vect;
    public GameObject[] _CardObj;
    GameObject _Curobj;
    public GameObject[] _PlacedCardPos,PlayerOutLine;

    void Start()
    {
        
    }

    // card Drag and drop
    public void Card_Begindrag(GameObject obj) {

        _Curobj = obj;
        _Vect = _Curobj.transform.position;
        print("1111---------");

    }
    public void Card_Ondrag() {
        //transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        print("2222---------");
        Vector3 point = (new Vector2(Input.mousePosition.x,Input.mousePosition.y));
        _Curobj.transform.position = point;
    }
    public void Card_Enddrag() {
        for (int i=0;i<_PlacedCardPos.Length;i++) {
            float Dist = Vector2.Distance(_Curobj.transform.position,_PlacedCardPos[i].transform.position);
            if (Dist > 2f) {
                print("hit---------");
                _Curobj.SetActive(false);
                PlayerOutLine[i].SetActive(false);
            }
        }

        _Curobj.transform.position=_Vect;
    }
}
