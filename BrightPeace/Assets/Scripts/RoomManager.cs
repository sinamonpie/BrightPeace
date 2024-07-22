using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public struct RoomPlayerData : INetworkStruct
{
    [Networked, Capacity(24)]
    public string Nickname { get => default; set { } }
    public PlayerRef PlayerRef;
    public bool IsConnected;
}

public enum EGameplayState
{
    Skirmish = 0,
    Running = 1,
    Finished = 2,
}

public class RoomManager : NetworkBehaviour
{
    private static RoomManager instance = null;

    public Camera[] camears;
    public List<Transform> patientSpawn;
    public Transform securitySpawn;

    public RoomPlayer[] gameObjects;

    [Networked]
    [Capacity(32)]
    [HideInInspector]
    public NetworkDictionary<PlayerRef, RoomPlayerData> PlayerData { get; }

    private bool _isNicknameSent;
    private static List<PlayerRef> _tempSpawnPlayers = new List<PlayerRef>();
    private static List<RoomPlayer> _tempSpawnedPlayers = new List<RoomPlayer>();

    [Networked]
    [HideInInspector]
    public EGameplayState State { get; set; }

    private List<RoomPlayerData> _tempPlayerData = new(16);

    public static RoomManager Instance
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
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    public override void Render()
    {
        if (Runner.Mode == SimulationModes.Server)
            return;

        // Every client must send its nickname to the server when the game is started.
        if (_isNicknameSent == false)
        {
            RPC_SetPlayerNickname(Runner.LocalPlayer, PlayerPrefs.GetString("Photon.Menu.Username"));
            _isNicknameSent = true;
        }
    }

    private void SpawnPlayer(PlayerRef playerRef)
    {
        if (PlayerData.TryGet(playerRef, out var playerData) == false)
        {
            playerData = new RoomPlayerData();
            playerData.Nickname = playerRef.ToString();
            playerData.IsConnected = false;
        }

        if (playerData.IsConnected == true)
            return;

        Debug.LogWarning($"{playerRef} connected.");

        playerData.IsConnected = true;
        PlayerData.Set(playerRef, playerData);

        //if(Runner.IsClient)
        //{
        //    SpawnSecurity(playerRef);
        //}
        //else
        //{
        //    SpawnPatient(playerRef);
        //}
        SpawnPatient(playerRef);
        //if (playerRef.PlayerId == 1)
        //{
        //    SpawnSecurity(playerRef);
        //}
        //else
        //{
        //    SpawnPatient(playerRef);
        //}

        RecalculateStatisticPositions();
    }

    private void SpawnSecurity(PlayerRef playerRef)
    {
        camears[0].gameObject.SetActive(true);
        camears[1].gameObject.SetActive(false);

        var spawnPoint = securitySpawn;
        var player1 = Runner.Spawn(gameObjects[0], spawnPoint.position, spawnPoint.rotation, playerRef);

        player1.gameObject.transform.SetParent(securitySpawn);

        Runner.SetPlayerObject(playerRef, player1.Object);
    }

    private void SpawnPatient(PlayerRef playerRef)
    {
        camears[0].gameObject.SetActive(false);
        camears[1].gameObject.SetActive(true);

        foreach (var trans in patientSpawn)
        {
            if (trans.childCount == 0)
            {
                var player2 = Runner.Spawn(gameObjects[1], trans.position, trans.rotation, playerRef);
                player2.gameObject.transform.SetParent(trans);

                Runner.SetPlayerObject(playerRef, player2.Object);
                break;
            }
        }
    }

    private void DespawnPlayer(PlayerRef playerRef, RoomPlayer player)
    {
        if (PlayerData.TryGet(playerRef, out var playerData) == true)
        {
            if (playerData.IsConnected == true)
            {
                Debug.LogWarning($"{playerRef} disconnected.");
            }

            playerData.IsConnected = false;
            PlayerData.Set(playerRef, playerData);
        }

        Runner.Despawn(player.Object);
    }


    private void RecalculateStatisticPositions()
    {
        if (State == EGameplayState.Finished)
            return;

        _tempPlayerData.Clear();

        foreach (var pair in PlayerData)
        {
            _tempPlayerData.Add(pair.Value);
        }

        for (int i = 0; i < _tempPlayerData.Count; i++)
        {
            var playerData = _tempPlayerData[i];

            PlayerData.Set(playerData.PlayerRef, playerData);
        }
    }


    public override void FixedUpdateNetwork()
    {
        UpdatePlayerConnections(Runner, SpawnPlayer, DespawnPlayer);


        if (State == EGameplayState.Skirmish && PlayerData.Count > 1)
        {
            State = EGameplayState.Running;
        }
    }

    public static void UpdatePlayerConnections(NetworkRunner runner, Action<PlayerRef> spawnPlayer, Action<PlayerRef, RoomPlayer> despawnPlayer)
    {
        _tempSpawnPlayers.Clear();
        _tempSpawnedPlayers.Clear();

        _tempSpawnPlayers.AddRange(runner.ActivePlayers);

        runner.GetAllBehaviours(_tempSpawnedPlayers);

        for (int i = 0; i < _tempSpawnedPlayers.Count; ++i)
        {
            RoomPlayer player = _tempSpawnedPlayers[i];
            PlayerRef playerRef = player.Object.InputAuthority;

            _tempSpawnPlayers.Remove(playerRef);

            if (runner.IsPlayerValid(playerRef) == false)
            {
                try
                {
                    despawnPlayer(playerRef, player);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }

        for (int i = 0; i < _tempSpawnPlayers.Count; ++i)
        {
            try
            {
                spawnPlayer(_tempSpawnPlayers[i]);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        _tempSpawnPlayers.Clear();
        _tempSpawnedPlayers.Clear();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, Channel = RpcChannel.Reliable)]
    private void RPC_SetPlayerNickname(PlayerRef playerRef, string nickname)
    {
        var playerData = PlayerData.Get(playerRef);
        playerData.Nickname = nickname;
        PlayerData.Set(playerRef, playerData);
    }
}
