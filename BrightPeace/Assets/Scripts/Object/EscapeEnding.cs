using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeEnding : MonoBehaviour
{
    // 여기서부터

    public bool crowBar_Trigger = false;
    public bool driver_Trigger = false;
    public bool TEST_Trigger = false;

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

    // 여기까지 필요없지만 다른데에서 참조해서 나중에 지우는게 나을듯

    private bool endingTrigger = false;

    // 활성화되면 열림
    public void EndingOK()
    {
        endingTrigger = true;
    }
}
