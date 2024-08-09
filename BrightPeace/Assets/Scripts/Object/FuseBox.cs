using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : MonoBehaviour
{
    public int puseNum = 0;
    public GameObject puseA;
    public GameObject puseB;
    public GameObject puseC;

    public GameObject lobbyDoor;

    public void InsertPuse()
    {
        puseNum++;
        SetPuse();
    }

    public void SetPuse()
    {
        switch (puseNum)
        {
            case 1:
                puseA.SetActive(true);
                break;
            case 2:
                puseB.SetActive(true);
                break;
            case 3:
                puseC.SetActive(true);
                break;
            default: break;
        }
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
        puseA.SetActive(false);
        puseB.SetActive(false);
        puseC.SetActive(false);

        lobbyDoor = GameObject.FindGameObjectWithTag("EndingLobby");
    }
}
