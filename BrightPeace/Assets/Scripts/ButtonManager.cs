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
        SoundManager.instance.PlaySoundEffect("ButtenClick");
        string nick = nickInput.text;
        if(!PhotonNetwork.IsConnectedAndReady)
        {
            alertManager.SetMessage("서버가 연결되지 않았습니다.\n잠시후 시도해주세요.");
        }
        if (nick.Trim().Equals(""))
        {
            alertManager.SetMessage("닉네임을 입력해주세요.");
            return;
        }

        PlayerPrefs.SetString("nick", nick);
        PhotonManager.Instance.JoinLobby(nick);
    }

    public void CreateRoomBtn()
    {
        SoundManager.instance.PlaySoundEffect("ButtenClick");
        selectWind.SetActive(false);
        selectSecurity.SetActive(true);
        PhotonManager.Instance.CreateRoom();
    }

    public void MatchingBtn()
    {
        SoundManager.instance.PlaySoundEffect("ButtenClick");
        selectWind.SetActive(false);
        selectMental.SetActive(true);
        PhotonManager.Instance.JoinMatching();
    }

    public void CancleMatchBtn()
    {
        SoundManager.instance.PlaySoundEffect("ButtenClick");
        selectMental.SetActive(false);
        selectWind.SetActive(true);
        PhotonManager.Instance.LeaveMatching();
    }

    public void CancleAndSelectBtn()
    {
        SoundManager.instance.PlaySoundEffect("ButtenClick");
        selectSecurity.SetActive(false);
        selectWind.SetActive(true);
    }

    public void LeaveRoomBtn()
    {
        SoundManager.instance.PlaySoundEffect("ButtenClick");
        PhotonManager.Instance.LeaveRoom();

    }

    public void LeaveLobbyBtn()
    {
        SoundManager.instance.PlaySoundEffect("ButtenClick");
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
