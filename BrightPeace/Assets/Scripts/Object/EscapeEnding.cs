using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeEnding : MonoBehaviour
{
    private bool crowBar_Trigger = false;
    private bool driver_Trigger = false;

    private bool endingTrigger = false;

    public void EndigTriggerCheck()
    {
        if(crowBar_Trigger && driver_Trigger)
        {
            endingTrigger = true;
        }
    }

    public void CrowBar_Trigger()
    {
        crowBar_Trigger = true;
    }

    public void Driver_Trigger()
    {
        driver_Trigger = true;
    }

    public bool EndigTriiger()
    {
        return endingTrigger;
    }

}
