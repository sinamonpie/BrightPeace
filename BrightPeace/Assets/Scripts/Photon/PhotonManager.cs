using Fusion;
using Fusion.Sockets;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks, INetworkRunnerCallbacks
{
    public string gameVersion = "0.1";
    public string nick = "";
    public string roomName = "";
    public int roomCount = 0;

    public bool isMatch = false;
    public bool isKicked = false;

    private static PhotonManager instance;
    

    public NetworkRunner _runner;
    private Dictionary<PlayerRef, NetworkObject> _spwanedRoomPlayers = new Dictionary<PlayerRef, NetworkObject>();

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
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }

    public void JoinLobby()
    {
        GameManager.Instance.LoadLobbyScene();
    }

    public void CreateRoom()
    {
        StartGame(GameMode.Host);
    }

    public void JoinMatching()
    {
        StartGame(GameMode.Client);
    }

    public void LeaveMatching()
    {

    }

    public void LeaveRoom()
    {

    }

    public void LeaveLobby()
    {

    }

    async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        //var sceneInfo = new NetworkSceneInfo();
        //var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        //if (scene.IsValid)
        //{
        //    sceneInfo.AddSceneRef(scene, LoadSceneMode.Single);
        //}    

        if (mode == GameMode.Host)
        {
            var result = await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                CustomLobbyName = "CustomLobby"
            });

            if (result.Ok)
            {
                // all good
                string sceneName = GameManager.Instance.sceneName[2];
                if(_runner.IsSceneAuthority)
                {
                    await _runner.LoadScene(sceneName, LoadSceneMode.Single, LocalPhysicsMode.None, true);
                }
            }
            else
            {
                Debug.LogError($"Failed to Start: {result.ShutdownReason}");
            }
        }
        else if (mode == GameMode.Client)
        {
            isMatch = false;

            var result = await _runner.JoinSessionLobby(SessionLobby.Custom, "CustomLobby");

            if (result.Ok)
            {
                // all good
                string sceneName = GameManager.Instance.sceneName[2];
                if (_runner.IsSceneAuthority)
                {
                    await _runner.LoadScene(sceneName, LoadSceneMode.Single, LocalPhysicsMode.None, true);
                }
            }
            else
            {
                Debug.LogError($"Failed to Start: {result.ShutdownReason}");
            }
        }
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Debug.Log("Join Player : " + runner.LocalPlayer.ToString());
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("PlayerLeft : " + runner.LocalPlayer.ToString());
        if (runner.IsServer)
            RoomManager.Server_Remove(runner, player);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("ShutDown : " + runner.LocalPlayer.ToString());
        if (shutdownReason == ShutdownReason.DisconnectedByPluginLogic)
        {
            if (runner.SceneManager != null)
            {
                if (runner.SceneManager.MainRunnerScene.IsValid() == true)
                {
                    SceneRef sceneRef = runner.SceneManager.GetSceneRef(runner.SceneManager.MainRunnerScene.name);
                    runner.SceneManager.UnloadScene(sceneRef);
                }
            }
        }
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        if (sessionList.Count > 0)
        {
            // Get first Session from the list
            var session = sessionList[0];

            Debug.Log($"Joining {session.Name}");

            // Join
            runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                SessionName = session.Name 
            });
        }

        Debug.Log($"Session List Updated with {sessionList.Count} session(s)");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
}
