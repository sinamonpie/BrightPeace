using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isClose = true;
    public bool DoorRotation = true;

    public GameObject pivot = null;

    private float doorAngle = 90;

    private void Start()
    {
        if(pivot == null)
        {
            pivot = this.gameObject;
        }
    }


    public void DoorControl()
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

    void UseDoor(GameObject pivot, float y)
    {
        if (!isClose)
        {
            y = -y;
        }
        pivot.transform.Rotate(0, y, 0);
        isClose = !isClose;
    }
}
