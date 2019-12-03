﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomController : MonoBehaviourPunCallbacks, IPunObservable
{
    public static RoomController roomController;

    [SerializeField] private int multiPlayerSceneIndex;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private Transform roomsContainer;
    [SerializeField] private Transform playersContainer;
    [SerializeField] private GameObject playerListingPrefab;
    [SerializeField] private TMP_Text roomNameDisplay;
    [SerializeField] private Image mainPanel;
    [SerializeField] private DataController dataControl;
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Sprite[] avatars;
    [SerializeField] private GameObject WaitingPanel;
    [SerializeField] private GameObject StartPanel;

    private GameObject templisting;
    private bool isEntered;

    public int characterSelected;
    public int enteredAt;
    public PhotonView pv;

    public void Awake()
    {
        if (roomController == null)
        {
            roomController = this;
        }
        else
        {
            if (roomController != this)
            {
                Destroy(roomController.gameObject);
                roomController = this;
            }
        }
        DontDestroyOnLoad(gameObject);

        characterSelected = 0;
    }

    void ClearRoomListings()
    {
        for (int i = roomsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(roomsContainer.GetChild(i).gameObject);
        }
    }

    [PunRPC]
    void ClearPlayerListings()
    {
        for (int i = playersContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }

    [PunRPC]
    void ListPlayer()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            templisting = Instantiate(playerListingPrefab, playersContainer);
            TMP_Text tempText = templisting.transform.GetChild(0).GetComponent<TMP_Text>();
            tempText.text = player.NickName;

            Image tempImg = templisting.transform.GetChild(1).GetComponent<Image>();
            int CharacterID = (int)player.CustomProperties["Avatar"];
            tempImg.sprite = avatars[CharacterID];
        }
    }

    public override void OnJoinedRoom()
    {
        Vector4 col = mainPanel.GetComponent<Image>().color;
        col.w = 0.0f;
        mainPanel.GetComponent<Image>().color = col;
        enteredAt = PhotonNetwork.PlayerList.Length;
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomNameDisplay.text = PhotonNetwork.CurrentRoom.Name;
        EnterRoom();
        Debug.Log("Joined");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Entered");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (isEntered)
        {
            if (PhotonNetwork.IsMasterClient)
                WaitingPanel.GetComponentInChildren<TextMeshProUGUI>().text = "waiting...";
        }
        else
        {
            EnterRoom();
        }

        characterSelected--;
        ClearPlayerListings();
        ListPlayer();
    }

    public void EnterRoom()
    {
        isEntered = true;
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(WaitForOthers());
        }

        dataControl.myCharacter = characters[(int)PhotonNetwork.LocalPlayer.CustomProperties["Avatar"]].name;

        pv.RPC("ClearPlayerListings", RpcTarget.All, null);
        pv.RPC("ListPlayer", RpcTarget.All, null);
        pv.RPC("CharacterJoined", RpcTarget.AllBuffered, null);
    }

    [PunRPC]
    public void CharacterJoined()
    {
        characterSelected++;
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (characterSelected == PhotonNetwork.PlayerList.Length)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel(multiPlayerSceneIndex);
            }
        }
    }

    IEnumerator RejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    public void BackOnClick()
    {
        ClearRoomListings();
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(RejoinLobby());
    }

    IEnumerator WaitForOthers()
    {
        while (characterSelected < PhotonNetwork.PlayerList.Length - 1)
        {
            Debug.Log("Waiting");
            WaitingPanel.GetComponentInChildren<TextMeshProUGUI>().text = "waiting...";
            yield return new WaitForSeconds(0.02f);
        }

        WaitingPanel.SetActive(false);
        StartPanel.SetActive(true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
