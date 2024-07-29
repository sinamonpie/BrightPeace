using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomPlayer : NetworkBehaviour
{
    public static RoomPlayer Local { get; private set; }

    [Networked, OnChangedRender(nameof(NicknameChanged))]
    public NetworkString<_16> Nickname { get; set; }

    public bool Ready;
    private ChangeDetector _changeDetector;

    public TMP_Text nickTxt;

    [Networked]
    public PlayerRef Ref { get; set; }

    [Networked]
    public byte Index { get; set; }

    public void Server_Init(PlayerRef pRef, byte index)
    {
        Ref = pRef;
        Index = index;
        Ready = false;
        if(pRef.PlayerId == 1)
        {
            RoomManager.Instance.SetSecurityCamera();
        }
        RoomManager.Instance.Rpc_GetPlayerCnt();
    }
    
    public override void Spawned()
    {
        base.Spawned();
        Debug.Log("State : " + Object.HasStateAuthority);
        Debug.Log("Input : " + Object.HasInputAuthority);
        if (Object.HasStateAuthority)
        {
            RoomManager.Server_Add(Runner, Object.InputAuthority, this);
        }

        if (Object.HasInputAuthority)
        {
            Local = this;
            Rpc_SetNickname(PlayerPrefs.GetString("nick"));
        }

        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        NicknameChanged();
    }

    void NicknameChanged()
    {
        nickTxt.text = Nickname.Value;
    }

    public void ReadyChanged()
    {
        if(!Ready)
        {
            RoomManager.Instance.readyTxt.text = "준비완료";
        }
        else
        {
            RoomManager.Instance.readyTxt.text = "준비";
        }
        Rpc_SetReady(!Ready);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void Rpc_SetNickname(string nick)
    {
        Nickname = nick;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_SetReady(bool ready)
    {
        Ready = ready;
        if(Runner.IsServer)
        {
            if (Ready)
                RoomManager.Instance.readyCount++;
            else
                RoomManager.Instance.readyCount--;

            RoomManager.Instance.Rpc_RelayReady(RoomManager.Instance.readyCount);
        }
    }

    //[Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    //public void Rpc_RelayReady(bool ready, PlayerRef playerRef)
    //{
    //    Ready = ready;
    //}
}
