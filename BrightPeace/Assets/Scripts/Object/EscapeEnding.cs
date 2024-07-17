using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeEnding : MonoBehaviour
{
    public GameObject triggerItem1;
    public GameObject triggerItem2;
    public GameObject triggerItem3;

    private bool trigger1 = false;
    private bool trigger2 = false;
    private bool trigger3 = false;

    public bool EndigTriggerCheck(GameObject trigger)
    {
        if(trigger == triggerItem1)
        {
            trigger1 = true;
        }
        else if(trigger == triggerItem2)
        {
            trigger2 = true;
        }
        else if(trigger == triggerItem3)
        {
            trigger3 = true;
        }

        if(trigger1 && trigger2 && trigger3)
        {
            return true;
        }
        return false;
    }
}
