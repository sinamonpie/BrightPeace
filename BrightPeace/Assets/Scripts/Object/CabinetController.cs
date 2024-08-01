using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetController : MonoBehaviour
{
    public void CabinetControl(GameObject player)
    {
        player.transform.position = this.transform.position;
    }
}
