using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public TMP_InputField nickInput;
    public AlertManager alertManager;

    public GameObject selectWind;
    public GameObject selectSecurity;
    public GameObject selectMental;

    public void JoinLobby()
    {
        if(!PhotonNetwork.IsConnected)
        {
            alertManager.SetMessage("서버와 연결되지않았습니다.\n잠시 후 다시 시도해주세요.");
            return;
        }

        string nick = nickInput.text;
        if (nick.Trim().Equals(""))
        {
            alertManager.SetMessage("닉네임을 입력해주세요.");
            return;
        }

        PhotonManager.Instance.JoinLobby(nick.Trim());
    }

    public void CreateRoomBtn()
    {
        selectWind.SetActive(false);
        selectSecurity.SetActive(true);
        PhotonManager.Instance.CreateRoom();
    }

    public void MatchingBtn()
    {
        selectWind.SetActive(false);
        selectMental.SetActive(true);
        PhotonManager.Instance.JoinMatching();
    }

    public void CancleMatchBtn()
    {
        selectMental.SetActive(false);
        selectWind.SetActive(true);
        PhotonManager.Instance.LeaveMatching();
    }

    public void CancleAndSelectBtn()
    {
        selectSecurity.SetActive(false);
        selectWind.SetActive(true);
    }

    public void LeaveLobbyBtn()
    {
        PhotonManager.Instance.LeaveLobby();
    }
}
