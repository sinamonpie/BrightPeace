using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public string gameVersion = "0.1";
    public string nick = "";
    public string roomName = "";
    public int roomCount = 0;

    public bool isMatch = false;
    public bool isKicked = false;

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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Connect();
    }

    private void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            nick = "";
            Debug.Log("Photon Connected");
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Disconnect");
        }
        else
            Debug.Log("Not Connect");
    }

    public void MasterClientDisconnect()
    {
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("kicked", true);

            player.Value.SetCustomProperties(hashtable);
        }
    }

    public void JoinLobby(string _nick)
    {
        if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InLobby)
        {
            nick = _nick;

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
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                if (player.Value != PhotonNetwork.LocalPlayer)
                {
                    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add("kicked", true);

                    player.Value.SetCustomProperties(hashtable);
                }
            }

        }
        PhotonNetwork.LeaveRoom();
    }

    public void JoinRoom(string _room)
    {
        if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InRoom)
        {
            roomName = _room;
            PhotonNetwork.JoinRoom(_room);
        }
    }

    //매칭하기
    public void JoinMatching()
    {
        isMatch = true;
        StartCoroutine(JoinRandomRoom());
    }

    //매칭취소
    public void LeaveMatching()
    {
        isMatch = false;
        StopCoroutine(JoinRandomRoom());
    }

    IEnumerator JoinRandomRoom()
    {
        while (true)
        {
            if (roomCount > 0)
            {
                PhotonNetwork.JoinRandomRoom();
                break;
            }

            yield return null;
        }
    }

    public void CreateRoom()
    {
        roomName = PhotonNetwork.LocalPlayer.UserId + "_" + Random.Range(0, 1000).ToString();
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 5 });
    }

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속");
        if (nick != null && !nick.Equals(""))
            JoinLobby(nick);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("연결 끊김 : {0}", cause);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Player : " + PhotonNetwork.LocalPlayer.NickName + " Join Lobby");

        PhotonChatManager.Instance.ChatConnect();

        if (!SceneManager.GetActiveScene().name.Equals(GameManager.Instance.sceneName[1]))
            GameManager.Instance.LoadLobbyScene();
        if (isMatch)
            JoinMatching();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player : " + PhotonNetwork.LocalPlayer.NickName + " Join Room : " + PhotonNetwork.CurrentRoom.Name);
        isMatch = false;

        GameManager.Instance.LoadRoomScene();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed : " + message);
        if (PhotonNetwork.IsConnected && !PhotonNetwork.InLobby)
            JoinLobby(nick);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join Random Room Failed : " + message);
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InLobby)
            {
                AlertManager alert = FindObjectOfType<AlertManager>();
                if (alert != null)
                {
                    alert.SetMessage("매칭이 오래 걸립니다.\n잠시 후 다시 시도해주세요.");
                }
            }
            else
            {
                JoinLobby(nick);
            }
        }
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Player " + PhotonNetwork.LocalPlayer.NickName + " Leave Lobby");
        nick = "";
        if (!SceneManager.GetActiveScene().name.Equals(GameManager.Instance.sceneName[0]))
            GameManager.Instance.LoadLoginScene();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Player : " + PhotonNetwork.LocalPlayer.NickName + "LeaveRoom");
        if (PhotonNetwork.IsConnected && !PhotonNetwork.InLobby)
            JoinLobby(nick);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Create Complete Player : " + PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed : " + message);
        if (PhotonNetwork.IsConnected && !PhotonNetwork.InLobby)
            JoinLobby(nick);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomCount = roomList.Count;
        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
            {
                roomCount--;
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer == PhotonNetwork.LocalPlayer)
        {
            if (changedProps["kicked"] != null)
            {
                if ((bool)changedProps["kicked"])
                {
                    string[] _removeProperties = new string[1];
                    _removeProperties[0] = "kicked";
                    isKicked = true;
                    PhotonNetwork.RemovePlayerCustomProperties(_removeProperties);
                    PhotonNetwork.LeaveRoom();
                }
            }
        }
    }
}
