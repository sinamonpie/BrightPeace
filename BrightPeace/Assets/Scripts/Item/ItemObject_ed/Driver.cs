using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Older;
public class Dirver : Item
{
    public override bool UseItem()
    {
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().CanDoorAction())
        {
            GameObject.FindGameObjectWithTag("Ending").GetComponent<EscapeEnding>().Driver_Trigger();
            GameObject.FindGameObjectWithTag("Ending").GetComponent<EscapeEnding>().EndigTriggerCheck();
            Debug.Log("Use Dirver");
            return true;
        }
        return base.UseItem();
    }
}
