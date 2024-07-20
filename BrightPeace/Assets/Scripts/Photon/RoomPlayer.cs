using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomPlayer : MonoBehaviour
{
    public TMP_Text nickTxt;
    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        if(PhotonNetwork.IsConnected)
        {
            if(pv.IsMine)
            {
                nickTxt.text = PhotonNetwork.LocalPlayer.NickName;
            }
            else
            {
                nickTxt.text = pv.Owner.NickName;
            }
        }
    }
}
