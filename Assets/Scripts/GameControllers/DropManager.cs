﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class DropManager : MonoBehaviour, IDropHandler
{
    public PlayerController myPlayer;
    public GameObject Item;
    public GameController _gameController;
    public GameplayManager _gameplayManager;

    public void Awake()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        _gameplayManager = GameObject.FindWithTag("GameplayManager").GetComponent<GameplayManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform rect = transform as RectTransform;

        if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
        {
            Item = DragManager.itemBeingDragged.gameObject;

            myPlayer.cardPlaced = true;
            myPlayer.placedCard = Item.GetComponent<DragManager>().CardID;
            _gameController.PlaceCards();
        }
    }
}
