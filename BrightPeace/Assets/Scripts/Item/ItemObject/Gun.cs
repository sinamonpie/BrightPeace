using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item
{
    public override bool UseItem()
    {
        return true;
    }
    public override void Equip()
    {
        GameObject parentObject = GameObject.FindWithTag("MainCamera");
        Transform childObject = parentObject.transform.GetChild(0);
        GameObject AimManger = childObject.gameObject;
        AimManger.SetActive(true);
    }

    public override void UnEquip()
    {
        GameObject parentObject = GameObject.FindWithTag("MainCamera");
        Transform childObject = parentObject.transform.GetChild(0);
        GameObject AimManger = childObject.gameObject;
        AimManger.SetActive(false);
    }
}
