using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static Transform itemBeingDragged;
    public int CardID;
    public Vector3 startPosition;
    public Transform startParent;

    private CanvasGroup canvasGroup;


    public void Start()
    {
        startPosition = transform.position;
        startParent = transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = transform;

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }

        itemBeingDragged = null;
    }
}
