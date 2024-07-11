using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isClose = true;

    public void DoorControl()
    {
        if(isClose)
        {
            DoorOpen();
        }
        else
        {
            DoorClose();
        }
    }

    void DoorOpen()
    {
        transform.Rotate(0, 90, 0);
        isClose = false;
    }

    void DoorClose()
    {
        transform.Rotate(0, -90, 0);
        isClose = true;
    }
}
