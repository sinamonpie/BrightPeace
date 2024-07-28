using Fusion;
using TMPro;
using UnityEngine;

public class RoomPlayer : NetworkBehaviour
{
    public static RoomPlayer Local { get; private set; }

    [Networked, OnChangedRender(nameof(NicknameChanged))]
    public NetworkString<_16> Nickname { get; set; }

    [Networked]
    public NetworkBool Ready { get; set; }

    public TMP_Text nickTxt;

    public bool isReady = false;

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

        NicknameChanged();
    }

    void NicknameChanged()
    {
        nickTxt.text = Nickname.Value;
    }

    public void ReadyChanged()
    {
        Ready = !Ready;
        Rpc_SetReady(Ready);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void Rpc_SetNickname(string nick)
    {
        Nickname = nick;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void Rpc_SetReady(bool ready, RpcInfo info = default)
    {
        Rpc_RelayReady(ready, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_RelayReady(bool ready, PlayerRef playerRef)
    {
        Ready = ready;
    }
}
