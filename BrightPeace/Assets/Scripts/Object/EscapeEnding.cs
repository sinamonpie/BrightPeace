using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeEnding : MonoBehaviour
{
    public GameObject triggerItem1;
    public GameObject triggerItem2;
    public GameObject triggerItem3;

    public ItemData test;

    private bool trigger1 = false;
    private bool trigger2 = false;
    private bool trigger3 = false;

    private bool endingTrigger = false;

    public void EndigTriggerCheck(GameObject trigger)
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
            endingTrigger = true;
        }
    }

    public void UseEndingDoor()
    {
        if (trigger1)
        {
            Debug.Log("엔딩 조건 1개 추가 충족");
        }
        else
        {
            Debug.Log("엔딩 조건 불충족");
        }
    }

    public void SolveTrigger()
    {
        trigger1 = true;
    }

    public bool EndigTriiger()
    {
        return endingTrigger;
    }

}
