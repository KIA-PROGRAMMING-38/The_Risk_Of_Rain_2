using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";

    private string userId = "Mati";
    private PhotonView photonView;
    private int readyPlayersCount = 0;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        // load scene automatically.
        PhotonNetwork.AutomaticallySyncScene = true;

        //check the version is the same each user 
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        // the number of commuincation, 30 per sec
        Debug.Log($"photon SendRate: {PhotonNetwork.SendRate}");
        //conncet sever

        PhotonNetwork.ConnectUsingSettings();


    }


    public Text connectionInfoText;
    public Button joinButton;

    private void Start()
    {

    }

    // after connecting to server...
    public override void OnConnectedToMaster()
    {
        Debug.Log($"Connected To Master");
        Debug.Log($"Photon Network.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
        //joinButton.interactable = true;
        //connectionInfoText.text = "Online : Connected to master server";
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"Photon Network.InLobby = {PhotonNetwork.InLobby}");

        PhotonNetwork.JoinRandomRoom(); // random match-making function.

    }

    // when random match failed...
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        networkStatus.text = "Room Status : Room Join Failed";
        Debug.Log($"JoinRandom Failed {returnCode} : {message}");

        RoomOptions rO = new RoomOptions();
        rO.MaxPlayers = 20; // max users 
        rO.IsOpen = true;
        rO.IsVisible = true; // can be checked on the room list. 

        //creat room
        PhotonNetwork.CreateRoom("First Room", rO);
    }

    //after creating room...(creating room is sucessful)
    public override void OnCreatedRoom()
    {
        Debug.Log("Room Status : Room Created!");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }


    [SerializeField]
    TextMeshProUGUI networkStatus;

    [SerializeField]
    TextMeshProUGUI notification;

    // after joing the room...
    public override void OnJoinedRoom()
    {
        networkStatus.text = "Room Status : Joined Room!";
        Debug.Log($"PhotonNetwrok.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        //check the joined users information
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        { // $ => string.Format(); 
            Debug.Log($"{player.Value.NickName},{player.Value.ActorNumber}"); //actor number is ID number of the user
        }
       
        
    }
    public void LoadGameScene()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("Main Scene");
        }
        else
        {
            notification.text = "one more player needed";
        }

    }

    public void OnClickReadyButton()
    {
        photonView.RPC("SetReadyState", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void SetReadyState()
    {
        readyPlayersCount++;

        if (readyPlayersCount == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            LoadGameScene();
        }
    }
}

