using Fusion;
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
        }
    }

    private void DespawnPlayer(PlayerRef playerRef, Player player)
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

}
