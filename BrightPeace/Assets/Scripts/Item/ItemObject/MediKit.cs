using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediKit : Item
{
    public override void UseItem()
    {
        base.UseItem();
        Debug.Log("Use Potion");
    }
}