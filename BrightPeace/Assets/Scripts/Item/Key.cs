using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    public override void UseItem()
    {
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().CanDoorAction())
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().UnlockDoor();
            Debug.Log("Use Key");
        }
        else
        {
            // 문을 바라보고 쓰시오 라는 메세지 띄우기
        }
    }
}
