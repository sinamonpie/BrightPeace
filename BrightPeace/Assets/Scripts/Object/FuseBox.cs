using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// 퓨즈 3개를 가져와 활성화를 시켜야함 퓨즈박스는 총 3개임
/// </summary>
public class FuseBox : MonoBehaviourPun
{
    public int puseNum = 0;
    public GameObject puseA;
    public GameObject puseB;
    public GameObject puseC;

    public GameObject lobbyDoor;

    public void InsertPuse()
    {
        photonView.RPC("RPC_InsertPuse", RpcTarget.All);
        photonView.RPC("RPC_SetPuse", RpcTarget.All, puseNum);
    }

    [PunRPC]
    void RPC_SetPuse(int num)
    {
        switch (num)
        {
            case 1:
                puseA.gameObject.SetActive(true);
                break;
            case 2:
                puseB.gameObject.SetActive(true);
                break;
            case 3:
                puseC.gameObject.SetActive(true);
                break;
            default: break;
        }
    }

    [PunRPC]
    void RPC_InsertPuse()
    {
        puseNum++;
    }

    public int GetPuseNum()
    {
        return puseNum;
    }

    public void UnlockLobbyDoor()
    {
        //로비문 열리는 트리거
        lobbyDoor.GetComponent<EscapeEnding>().EndingOK();
    }

    public int PuseBoxCheck()
    {
        return lobbyDoor.GetComponent<EscapeEnding>().PuseBoxEndingCheck();
    }

    public void ClearPuseBox()
    {
        lobbyDoor.GetComponent<EscapeEnding>().ClearPuseBox();
    }

    void Start()
    {
        lobbyDoor = GameObject.FindGameObjectWithTag("EndingLobby");
    }
}
