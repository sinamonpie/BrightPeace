using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player = null;
    public List<string> sceneName = new List<string>();

    public bool isGameStart = false;
    public UserRole role;
    public int endding = 0;
    public int playKill = 0;

    private static GameManager instance;

    public static GameManager Instance
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

    private void Start()
    {
        SoundManager.instance.PlayBGM("로그인");
    }

    public void SetRole(UserRole _role)
    {
        role = _role;
    }

    public void SetEnding(int endIdx)
    {
        isGameStart = false;
        endding = endIdx;
    }

    public void LoadLoginScene()
    {
        SceneManager.LoadSceneAsync(0);
        SoundManager.instance.PlayBGM("로그인");
    }

    public void LoadLobbyScene()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        SceneManager.LoadSceneAsync(1);
        SoundManager.instance.PlayBGM("로비");
    }

    public void LoadRoomScene()
    {
        PhotonNetwork.LoadLevel(sceneName[2]);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonChatManager.Instance.JoinRoomChannel(PhotonNetwork.CurrentRoom.Name);
        playKill = 0;
        SoundManager.instance.PlayBGM("룸");
    }

    public void LoadGamescene()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(sceneName[3]);
    }

    public void LoadEndding()
    {
        SceneManager.LoadSceneAsync(4);
    }
}
