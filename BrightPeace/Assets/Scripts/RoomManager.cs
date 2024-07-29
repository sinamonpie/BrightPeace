using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : NetworkBehaviour, ISpawned, IDespawned, IPlayerJoined, IPlayerLeft
{
    public static RoomManager Instance { get; private set; }

    public Camera[] camears;
    public List<Transform> patientSpawn;
    public Transform securitySpawn;

    public RoomPlayer[] gameObjects;

    public NetworkObject playerObject;

    public bool isReadySlider = false;
    public int readyCount = 0;

    [Networked, Capacity(5)]
    NetworkDictionary<PlayerRef, RoomPlayer> ObjectByRef { get; }

    public Button readyBtn;
    public TMP_Text readyTxt;
    public ReadySlider readySlider;
    public Animation readyAnim;

    public override void Spawned()
    {
        base.Spawned();
        Instance = this;
        Runner.AddCallbacks(PhotonManager.Instance);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
        Instance = null;
        runner.RemoveCallbacks(PhotonManager.Instance);
    }

    public void SetSecurityCamera()
    {
        camears[0].gameObject.SetActive(true);
        camears[1].gameObject.SetActive(false);

        //readyBtn.interactable = false;
        readySlider.SetMaxReadyCnt();
        readyTxt.text = "게임시작";
    }

    public void PlayerJoined(PlayerRef player)
    {
        SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        RoomPlayer leftPlayer = RoomManager.GetPlayer(player);
        if (leftPlayer != null)
        {
            if (leftPlayer.Ready)
                readyCount--;
            DespawnPlayer(player, leftPlayer);
        }
    }

    public void SpawnPlayer(PlayerRef playerRef)
    {
        if (playerRef.PlayerId == 1)
        {
            SpawnSecurity(playerRef);
        }
        else
        {
            SpawnPatient(playerRef);
        }
    }

    private void SpawnSecurity(PlayerRef playerRef)
    {
        var spawnPoint = securitySpawn;
        var player1 = Runner.Spawn(gameObjects[0], spawnPoint.position, spawnPoint.rotation, playerRef);

        player1.gameObject.transform.SetParent(securitySpawn);

        Runner.SetPlayerObject(playerRef, player1.Object);
    }

    private void SpawnPatient(PlayerRef playerRef)
    {
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

    public void DespawnPlayer(PlayerRef playerRef, RoomPlayer player)
    {
        Runner.Despawn(player.Object);
    }

    public static void Server_Add(NetworkRunner runner, PlayerRef pRef, RoomPlayer pObj)
    {
        Debug.Assert(runner.IsServer);

        if (Instance.GetAvailable(out byte index))
        {
            Instance.ObjectByRef.Add(pRef, pObj);
            pObj.Server_Init(pRef, index);
        }
        else
        {
            Debug.LogWarning($"Unable to register player {pRef}", pObj);
        }
    }

    public static void Server_Remove(NetworkRunner runner, PlayerRef pRef)
    {
        if (Instance.ObjectByRef.Remove(pRef) == false)
        {
            Debug.LogWarning("Could not remove player from registry");
        }
    }

    bool GetAvailable(out byte index)
    {
        if (ObjectByRef.Count == 0)
        {
            index = 0;
            return true;
        }
        else if (ObjectByRef.Count == 5)
        {
            index = default;
            return false;
        }

        byte[] indices = ObjectByRef.OrderBy(kvp => kvp.Value.Index).Select(kvp => kvp.Value.Index).ToArray();

        for (int i = 0; i < indices.Length - 1; i++)
        {
            if (indices[i + 1] > indices[i] + 1)
            {
                index = (byte)(indices[i] + 1);
                return true;
            }
        }

        index = (byte)(indices[indices.Length - 1] + 1);
        return true;
    }

    public static RoomPlayer GetPlayer(PlayerRef pRef)
    {
        if (HasPlayer(pRef))
            return Instance.ObjectByRef.Get(pRef);
        return null;
    }

    public static bool HasPlayer(PlayerRef pRef)
    {
        return Instance.ObjectByRef.ContainsKey(pRef);
    }

    public void OnReady()
    {
        // readyCount == 4 && 
        if (Runner.IsServer &&!isReadySlider)
        {
            StartGame();
        }
        else
        {
            RoomPlayer.Local.ReadyChanged();
        }
    }

    public void StartGame()
    {
        Runner.SessionInfo.IsOpen = false;
        Runner.LoadScene(SceneRef.FromIndex(3), UnityEngine.SceneManagement.LoadSceneMode.Single);
        Rpc_RelayStart();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_RelayStart()
    {
        StopAllCoroutines();
        readyAnim.Play("Start");
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void Rpc_GetPlayerCnt(RpcInfo info = default)
    {
        Rpc_GetReady(readyCount, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_GetReady(int readyCnt, PlayerRef playerRef)
    {
        if (playerRef == Runner.LocalPlayer)
        {
            readyCount = readyCnt;

            StopAllCoroutines();
            StartCoroutine(SetReadyTimeTicker(readyCount));
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_RelayReady(int readyCnt)
    {
        readyCount = readyCnt;

        StopAllCoroutines();
        StartCoroutine(SetReadyTimeTicker(readyCount));
    }

    IEnumerator SetReadyTimeTicker(int readyCnt)
    {
        isReadySlider = true;
        float startCnt = readySlider.GetReadyCnt();
        float endCnt = (float)readyCnt;
        float current = startCnt;
        if (startCnt < endCnt)
        {
            while(current <= endCnt)
            {
                current += Time.deltaTime;

                readySlider.SetReadyCnt(current);

                yield return null;
            }
        }
        else
        {
            while (current >= endCnt)
            {
                current -= Time.deltaTime; 
                
                readySlider.SetReadyCnt(current);

                yield return null;
            }
        }
        readySlider.SetReadyCnt(endCnt);
        isReadySlider = false;
    }
}
