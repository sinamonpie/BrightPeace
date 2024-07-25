using Fusion;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomPlayer : NetworkBehaviour
{
    public static RoomPlayer Local { get; private set; }

    [Networked, OnChangedRender(nameof(NicknameChanged))]
    public NetworkString<_16> Nickname { get; set; }

    public TMP_Text nickTxt;

    [Networked]
    public PlayerRef Ref { get; set; }
    [Networked]
    public byte Index { get; set; }

    public void Server_Init(PlayerRef pRef, byte index)
    {
        Ref = pRef;
        Index = index;
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
            if(RoomManager.Instance.PlayerData.TryGet(Object.Runner.LocalPlayer, out RoomPlayerData value))
            {
                Rpc_SetNickname(value.Nickname);
            }
        }

        NicknameChanged();
    }

    void NicknameChanged()
    {
        nickTxt.text = Nickname.Value;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void Rpc_SetNickname(string nick)
    {
        Nickname = nick;
    }
}
