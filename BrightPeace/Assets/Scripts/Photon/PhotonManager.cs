using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public string gameVersion = "0.1";
    public string nick = "";

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
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Connect();
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

    public void DisConnect()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        else
            Debug.Log("Not Connect");
    }



    public void JoinLobby()
    {
        if(PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InLobby)
        {
            nick = "Player" + Random.Range(0, 1000).ToString();
            PhotonNetwork.LocalPlayer.NickName = nick.Trim();

            PhotonNetwork.JoinLobby();
        }
        else
        {
            Debug.Log("Not Connected Or In Lobby");
        }
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
        base.OnJoinedLobby();
        Debug.Log("Player : " + nick + " Join Lobby");
    }
}
