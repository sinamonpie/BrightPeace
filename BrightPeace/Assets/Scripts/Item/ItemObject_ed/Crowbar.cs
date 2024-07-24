using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : Item
{
    public override bool UseItem()
    {
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().CanDoorAction())
        {

        }
        return base.UseItem();
    }
}
