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

    [SerializeField]
    private GameObject lockDoorUI;

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

        if (!unlockDoor)
        {
            if (pv != null)
            {
                lockDoorUI = InGameManager.Instance.doorLockUI;
                lockDoorUI.GetComponent<DoorUseKeyUI>().isDoor = true;
                Instantiate(lockDoorUI, this.transform);
            }
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
        SoundManager.instance.PlayEffectAtPoint("OpenDoor", transform.position);
        isClose = !isClose;
    }

    public bool UseableDoor()
    {
        return unlockDoor;
    }

    public void UnlockDoor()
    {
        SoundManager.instance.PlayEffectAtPoint("CloseDoor", transform.position);
        unlockDoor = true;
    }

    public void SetDoorUI(float time)
    {
        pv.RPC("RPC_DoorUI", RpcTarget.All, time);
    }

    [PunRPC]
    void RPC_DoorUI(float time)
    {
        GetComponentInChildren<DoorUseKeyUI>().DoorUI(time);
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
