using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public string gameVersion = "0.1";
    public string nick = "";
    public string roomName = "";
    public int roomCount = 0;

    private bool isMatch = false;

    private static PhotonManager instance;

    public static PhotonManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        Connect();
    }

    private void Update()
    {
    }

    public void LeaveRoomBtn()
    {
        LeaveRoom();
    }

    public void CreateJoinRoom()
    {
        if (roomCount == 0)
            CreateRoom();
        else
            JoinMatching();
    }

    private void Connect()
    {
        if(PhotonNetwork.IsConnected)
        {
            Debug.Log("Photon Connected");
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void SetPlayer()
    {

    }

    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        else
            Debug.Log("Not Connect");
    }

    public void JoinLobby(string nick = null)
    {
        if(PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InLobby)
        {
            if(nick == null || nick.Equals(""))
            {
                nick = "Player" + Random.Range(0, 1000).ToString();
            }
            PhotonNetwork.LocalPlayer.NickName = nick.Trim();
            PhotonNetwork.JoinLobby();
        }
        else
        {
            Debug.Log("Not Connected Or In Lobby");
        }
    }

    public void LeaveLobby()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        else
        {
            Debug.Log("Not Lobby");
        }
    }

    public void LeaveRoom()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LeaveRoom(false);
            //foreach(var player in PhotonNetwork.PlayerListOthers)
            //{
            //}
        }
    }

    public void JoinRoom(string room)
    {
        if(PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InRoom)
        {
            roomName = room;
            PhotonNetwork.JoinRoom(room);
        }
    }

    //¸ÅÄªÇÏ±â
    public void JoinMatching()
    {
        isMatch = true;
        StartCoroutine(JoinRandomRoom());
    }

    //¸ÅÄªÃë¼Ò
    public void LeaveMatching()
    {
        isMatch = false;
        StopCoroutine(JoinRandomRoom());
    }

    IEnumerator JoinRandomRoom()
    {
        while(true)
        {
            if(roomCount > 0)
            {
                PhotonNetwork.JoinRandomRoom();
                break;
            }

            yield return null;
        }
    }

    public void CreateRoom()
    {
        roomName = nick + "_" + Random.Range(0, 1000).ToString();
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 5 });
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("¼­¹ö Á¢¼Ó");
        JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("¿¬°á ²÷±è : {0}", cause);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Player : " + nick + " Join Lobby");
        if (isMatch)
            JoinMatching();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player : " + nick + " Join Room");
        isMatch = false;

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed : " + message);
        if (PhotonNetwork.IsConnected)
        {
            JoinLobby();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join Random Room Failed : " + message);
        if (PhotonNetwork.IsConnected)
            JoinLobby();
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Player " + nick + " Leave Lobby");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Player : " + nick + "LeaveRoom");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Create Complete Player : " + nick);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed : " + message);
        if (PhotonNetwork.IsConnected)
            JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomCount = roomList.Count;
    }
}
