using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player = null;
    public List<string> sceneName = new List<string>();

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

    public void LoadLoginScene()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadRoomScene()
    {
        SceneManager.LoadScene(2);
    }
}
