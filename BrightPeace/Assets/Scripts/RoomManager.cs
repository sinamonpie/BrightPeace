using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using System;
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

    public TMP_InputField chat;
    public Transform chatTrans;
    public GameObject chatObject;
    public GameObject noticeObject;
    public GameObject privateChatObject;

    public GameObject voice;

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


        PhotonNetwork.Instantiate(voice.name, new Vector3(0,0,0), Quaternion.identity);
        pv.RPC("setNotice", RpcTarget.All, "[" + PhotonNetwork.NickName + "] 님 께서 입장하셨습니다.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (chat.text != "")
            {

                if (chat.text.Contains(">>"))
                {
                    string[] chatTxt = chat.text.Split(">>");

                    string nick = chatTxt[0];
                    string message = chatTxt[1];
                    PhotonChatManager.Instance.SendPrivateMessage(nick, message);
                }
                else
                {
                    PhotonChatManager.Instance.SendChatMessage(chat.text);
                }
                chat.text = "";
            }
            chat.ActivateInputField();
        }
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

        setNotice("[" + otherPlayer.NickName + "] 님 께서 퇴장하셨습니다.");

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
        SoundManager.instance.PlaySoundEffect("ButtenClick");
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("ReceiveStart", RpcTarget.All);
        }
    }

    void btnReady()
    {
        SoundManager.instance.PlaySoundEffect("ButtenClick");
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
        bool isStart = true;
        foreach (GameObject _player in GameObject.FindGameObjectsWithTag("RoomPlayer"))
        {
            if (!_player.GetComponent<RoomPlayer>().Ready)
            {
                isStart = false;
            }
        }

        
        foreach (GameObject _speaker in GameObject.FindGameObjectsWithTag("Speaker"))
        {
            _speaker.GetComponent<AudioSource>().volume = 0;
        }

        SoundManager.instance.StopBGM();

        if (isStart)
        {
            StopAllCoroutines();
            readyAnim.Play("Start");
            StartCoroutine(SetStart());
        }
        else
        {
            setNotice("준비를 하지 않은 플레이어가 있습니다.");
        }
    }

    IEnumerator SetStart()
    {
        yield return new WaitForSeconds(2.0f);
        loadding.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.LoadGamescene();
        }
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


    // Photon Chat

    [PunRPC]
    public void setNotice(string message)
    {
        GameObject _chat = Instantiate(noticeObject, chatTrans);
        _chat.GetComponent<ChatObjectOption>().SetNotice(message);
    }

    public void setUserChat(string nick, string message, bool isMaster)
    {
        GameObject _chat = Instantiate(chatObject, chatTrans);
        _chat.GetComponent<ChatObjectOption>().SetMessage(nick, message, isMaster);
    }

    public void setPrivateUserChat(string sender, string message, bool isMine)
    {
        GameObject _chat = Instantiate(chatObject, chatTrans);
        _chat.GetComponent<ChatObjectOption>().SetPrivateMessage(sender, message, isMine);
    }
}
