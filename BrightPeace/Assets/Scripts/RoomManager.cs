using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public struct RoomPlayerData : INetworkStruct
{
    [Networked, Capacity(24)]
    public string Nickname { get => default; set { } }
    public bool IsConnected;
}

public class RoomManager : NetworkBehaviour
{
    private static RoomManager instance = null;

    public Camera[] camears;
    public List<Transform> patientSpawn;
    public Transform securitySpawn;

    public GameObject[] gameObjects;

    [Networked]
    [Capacity(32)]
    [HideInInspector]
    public NetworkDictionary<PlayerRef, RoomPlayerData> PlayerData { get; }

    private static List<PlayerRef> _tempSpawnPlayers = new List<PlayerRef>();
    private static List<RoomPlayer> _tempSpawnedPlayers = new List<RoomPlayer>();

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


    private void SpawnPlayer(PlayerRef playerRef)
    {
        if (PlayerData.TryGet(playerRef, out var playerData) == false)
        {
            playerData = new RoomPlayerData();
            playerData.Nickname = playerRef.ToString();
            playerData.IsConnected = false;

            PlayerData.Set(playerRef, playerData);
        }

        if (Runner.IsServer)
        {
        }
        else
        {
        }
    }

    private void SpawnSecurity(PlayerRef playerRef)
    {
        Camera.main.transform.position = camears[0].transform.position;

        var spawnPoint = securitySpawn;
        var player = Runner.Spawn(gameObjects[0], spawnPoint.position, spawnPoint.rotation, playerRef);

        player.gameObject.transform.SetParent(securitySpawn);
    }

    private void SpawnPatient(PlayerRef playerRef)
    {
        Camera.main.transform.position = camears[1].transform.position;

        foreach (var trans in patientSpawn)
        {
            if (trans.childCount == 0)
            {
                var spawnPoint = trans;

                var player2 = Runner.Spawn(gameObjects[1], spawnPoint.position, spawnPoint.rotation, playerRef);
                player2.gameObject.transform.SetParent(spawnPoint);
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

    public override void FixedUpdateNetwork()
    {
        UpdatePlayerConnections(Runner, SpawnPlayer, DespawnPlayer);
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
}
