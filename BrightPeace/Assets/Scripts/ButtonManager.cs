using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public TMP_InputField nickInput;
    public AlertManager alertManager;

    public GameObject selectWind;
    public GameObject selectSecurity;
    public GameObject selectMental;

    public void JoinLobby()
    {
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

    public void LeaveRoomBtn()
    {
        PhotonManager.Instance.LeaveRoom();
    }

    public void LeaveLobbyBtn()
    {
        PhotonManager.Instance.LeaveLobby();
    }

    public void Update()
    {
        if(PhotonNetwork.InLobby && SceneManager.GetActiveScene().name.Equals(GameManager.Instance.sceneName[1]))
        {
            if (PhotonManager.Instance.isKicked)
            {
                alertManager.SetMessage("방장이 나갔습니다.\n다시 매칭해주세요.");
                PhotonManager.Instance.isKicked = false;
            }
        }
    }
}
