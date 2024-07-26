using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Older;
public class Knife : Item
{

    public override bool UseItem()
    {
        Debug.Log("Use Knife");
        return true;
    }

    public override void Equip()
    {
        GameObject parentObject = GameObject.FindWithTag("MainCamera");
        Transform childObject = parentObject.transform.GetChild(1);
        GameObject AimManger = childObject.gameObject;
        AimManger.SetActive(true);
    }
}
