using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance { get; private set; }

    public Camera[] camears;
    public List<Transform> patientSpawn;
    public Transform securitySpawn;

    public GameObject[] gameObjects;

    private PhotonView pv;

    public Button readyBtn;
    public Button leaveBtn;

    public TMP_Text readyTxt;
    public ReadySlider readySlider;
    public Animation readyAnim;
    public int readyCount = 0;
    public bool isReadySlider = false;

    public bool Ready = false;
    public GameObject loadding;

    public Player masterClient;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            readyTxt.text = "게임 시작";
            readyBtn.onClick.AddListener(btnStart);
        }
        else
        {
            readyTxt.text = "준비";
            readyBtn.onClick.AddListener(btnReady);
        }

        masterClient = PhotonNetwork.MasterClient;

        if (leaveBtn != null)
            leaveBtn.onClick.AddListener(PhotonManager.Instance.LeaveRoom);

        PlayerCameraSetting();
        InPlayerInfo();
    }


    void PlayerCameraSetting()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            camears[0].gameObject.SetActive(true);
            camears[1].gameObject.SetActive(false);
        }
        else
        {
            camears[0].gameObject.SetActive(false);
            camears[1].gameObject.SetActive(true);
        }
    }

    void InPlayerInfo()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("RoomPlayer"))
        {
            Destroy(obj);
        }

        Dictionary<int, Photon.Realtime.Player> playerList = PhotonNetwork.CurrentRoom.Players;
        foreach (var player in playerList)
        {
            if (player.Value.IsMasterClient)
            {
                GameObject obj = Instantiate(gameObjects[0], securitySpawn);
                obj.SetActive(true);
                obj.GetComponent<RoomPlayer>().playerInfo = player.Value;
                obj.GetComponent<RoomPlayer>().setInfo();
            }
            else
            {
                foreach (var trans in patientSpawn)
                {
                    if (trans.childCount == 0)
                    {
                        GameObject obj = Instantiate(gameObjects[1], trans);
                        obj.SetActive(true);
                        obj.GetComponent<RoomPlayer>().playerInfo = player.Value;
                        obj.GetComponent<RoomPlayer>().setInfo();
                        break;
                    }
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (newPlayer.IsMasterClient)
        {
            GameObject obj = Instantiate(gameObjects[0], securitySpawn);
            obj.SetActive(true);
            obj.GetComponent<RoomPlayer>().playerInfo = newPlayer;
            obj.GetComponent<RoomPlayer>().setInfo();
        }
        else
        {
            foreach (var trans in patientSpawn)
            {
                if (trans.childCount == 0)
                {
                    GameObject obj = Instantiate(gameObjects[1], trans);
                    obj.SetActive(true);
                    obj.GetComponent<RoomPlayer>().playerInfo = newPlayer;
                    obj.GetComponent<RoomPlayer>().setInfo();
                    break;
                }
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Left Player Master ? " + otherPlayer.IsMasterClient);
        Debug.Log("otherPlayer ? " + otherPlayer);
        Debug.Log("materClient ? " + masterClient);

        Debug.Log(otherPlayer.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            if (otherPlayer == masterClient)
            {
                PhotonManager.Instance.MasterClientDisconnect();
                return;
            }

            readyCount--;
            pv.RPC("SetReadyCount", RpcTarget.AllBuffered, readyCount);
        }

        foreach (GameObject _player in GameObject.FindGameObjectsWithTag("RoomPlayer"))
        {
            if (_player.GetComponent<RoomPlayer>().playerInfo == otherPlayer)
            {
                Destroy(_player);
            }
            else
            {
                _player.GetComponent<RoomPlayer>().setInfo();
            }
        }
    }

    void btnStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {

            bool isStart = true;
            foreach (GameObject _player in GameObject.FindGameObjectsWithTag("RoomPlayer"))
            {
                if (!_player.GetComponent<RoomPlayer>().Ready)
                {
                    isStart = false;
                }
            }
            if (isStart)
            {
                pv.RPC("ReceiveStart", RpcTarget.All);
            }
        }
    }

    void btnReady()
    {
        Ready = !Ready;

        if(Ready)
            readyTxt.text = "준비 완료";
        else
            readyTxt.text = "준비";

        pv.RPC("ReceiveReady", RpcTarget.AllBuffered, Ready, PhotonNetwork.LocalPlayer.NickName);
    }

    [PunRPC]
    void ReceiveReady(bool isReady, string nick)
    {
        foreach (GameObject _player in GameObject.FindGameObjectsWithTag("RoomPlayer"))
        {
            if (_player.GetComponent<RoomPlayer>().playerInfo.NickName == nick)
            {
                _player.GetComponent<RoomPlayer>().Ready = isReady;
            }
            _player.GetComponent<RoomPlayer>().setReady();
        }

        if(PhotonNetwork.IsMasterClient)
        {
            if (isReady)
                readyCount++;
            else
                readyCount--;

            if (PhotonNetwork.CurrentRoom.PlayerCount <= readyCount)
                readyCount = PhotonNetwork.CurrentRoom.PlayerCount;

            pv.RPC("SetReadyCount", RpcTarget.AllBuffered, readyCount);
        }
    }

    [PunRPC]
    void SetReadyCount(int _cnt)
    {
        StopAllCoroutines();

        readyCount = _cnt;
        StartCoroutine(SetReadyTimeTicker(readyCount));
    }

    [PunRPC]
    void ReceiveStart()
    {
        StopAllCoroutines();
        readyAnim.Play("Start");
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SetStart());
        }
    }

    IEnumerator SetStart()
    {
        yield return new WaitForSeconds(2.0f);
        loadding.SetActive(true);
        GameManager.Instance.LoadGamescene();
    }

    IEnumerator SetReadyTimeTicker(int readyCnt)
    {
        isReadySlider = true;
        float startCnt = readySlider.GetReadyCnt();
        float endCnt = (float)readyCnt;
        float current = startCnt;
        if (startCnt < endCnt)
        {
            while (current <= endCnt)
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
