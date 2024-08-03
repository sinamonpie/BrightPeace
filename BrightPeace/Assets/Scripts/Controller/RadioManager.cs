using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RadioManager : MonoBehaviourPun
{
    [SerializeField]
    private VoiceManager voiceManager;
    private bool isUsingRadio = false;
    private static Player currentRadioUser = null;
    void Start()
    {
        voiceManager = FindObjectOfType<VoiceManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            OnPressTalkButton();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnReleaseTalkButton();
        }
    }

    public void OnPressTalkButton()
    {
        if (!PhotonNetwork.IsConnected || isUsingRadio)
            return;

        // RPC 호출: 서버에 무전기 사용 요청
        photonView.RPC("RequestToUserRadio", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    public void RequestToUserRadio(Player requestingPlayer)
    {
        // 무전기 사용 권한을 체크하고, 한 유저에게만 권한 부여
        if (CanUseWRadio(requestingPlayer))
        {
            currentRadioUser = requestingPlayer;
            photonView.RPC("AllowUseRadio", requestingPlayer);
        }
    }

    private bool CanUseWRadio(Player player)
    {
        return currentRadioUser == null;
    }

    [PunRPC]
    public void AllowUseRadio()
    {
        // 로컬에서 무전기 사용 허가
        voiceManager.SetTransmitEnabled(true);
        isUsingRadio = true;
    }

    public void OnReleaseTalkButton()
    {
        if (!PhotonNetwork.IsConnected || !isUsingRadio)
            return;
        
        // 무전기 버튼을 놓았을 때 RPC 호출
        photonView.RPC("ReleaseRadio", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    public void ReleaseRadio(Player releasingPlayer)
    {
        // 무전기 사용 중지 로직
        if (IsCurrentlyUsingRadio(releasingPlayer))
        {
            currentRadioUser = null;
            photonView.RPC("StopUsingRadio", releasingPlayer);
        }
    }

    [PunRPC]
    public void StopUsingRadio()
    {
        // 로컬에서 무전기 사용 중지
        voiceManager.SetTransmitEnabled(false);
        isUsingRadio = false;
    }

    private bool IsCurrentlyUsingRadio(Player player)
    {
        return currentRadioUser == player;
    }
}
