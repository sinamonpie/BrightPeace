using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isClose = true;
    public bool DoorRotation = true;

    public GameObject pivot = null;

    public bool unlockDoor = true;

    private float doorAngle = 90;
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(pivot == null)
        {
            pivot = this.gameObject;
        }
    }


    public void DoorControl()
    {
        pv.RPC("RPC_DoorControl", RpcTarget.All);
    }

    void UseDoor(GameObject pivot, float y)
    {
        if (!isClose)
        {
            y = -y;
        }
        pivot.transform.Rotate(0, this.transform.rotation.y + y, 0);
        isClose = !isClose;
    }

    public bool UseableDoor()
    {
        return unlockDoor;
    }

    public void UnlockDoor()
    {
        unlockDoor = true;
    }

    [PunRPC]
    public void RPC_DoorControl()
    {
        if (DoorRotation)
        {
            // 일반 문
            UseDoor(pivot, -doorAngle);
        }
        else
        {
            // 피벗없는 문
            UseDoor(pivot, doorAngle);
        }
    }
}
