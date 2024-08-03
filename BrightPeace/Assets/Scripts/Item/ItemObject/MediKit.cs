using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Older;
public class MediKit : Item
{
    public override bool UseItem()
    {
        Debug.Log("Use Potion");
        return true;
    }
}