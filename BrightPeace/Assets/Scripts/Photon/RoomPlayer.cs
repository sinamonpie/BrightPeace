using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomPlayer : MonoBehaviour
{
    public static RoomPlayer Local { get; private set; }
    public Player playerInfo;

    public TMP_Text nickTxt;

    public bool Ready;

    bool setting = false;

    public void setInfo()
    {
        if (playerInfo.IsMasterClient)
        {
            Ready = true;
            nickTxt.text = $"{playerInfo.NickName}";
        }
        else
        {
            if (Ready)
            {
                nickTxt.text = $"{playerInfo.NickName}";
            }
            else
            {
                nickTxt.text = $"{playerInfo.NickName}";
            }
        }

        if (!setting)
        {
            setting = true;
        }
    }


    public void setReady()
    {
        if (Ready)
        {
            nickTxt.text = $"{playerInfo.NickName}" + " Ready";
        }
        else
        {
            nickTxt.text = $"{playerInfo.NickName}";
        }
    }
}
