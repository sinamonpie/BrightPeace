using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlertManager : MonoBehaviour
{
    public TMP_Text message;

    public void SetMessage(string _message)
    {
        message.text = _message;
        gameObject.SetActive(true);
    }

    public void OKBtn()
    {
        gameObject.SetActive(false);

        if(!PhotonNetwork.IsConnectedAndReady)
        {
            if (!SceneManager.GetActiveScene().name.Equals(GameManager.Instance.sceneName[0]))
                GameManager.Instance.LoadLoginScene();
        }
    }
}
