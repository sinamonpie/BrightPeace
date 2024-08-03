using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CabinetController : MonoBehaviour
{
    private bool hideInCabinet = false;
    Vector3 getOutCabinetPos;

    public void CabinetControl(GameObject player)
    {
        if (hideInCabinet)
        {
            player.transform.position = getOutCabinetPos;
            hideInCabinet = false;
        }
        else
        {
            getOutCabinetPos = player.transform.position;
            player.transform.position = this.transform.position;
            hideInCabinet = true;
        }
    }

    public bool HideInCabinet()
    {
        return hideInCabinet;
    }
}
