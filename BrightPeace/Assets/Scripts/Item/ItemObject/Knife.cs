using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Item
{
    public override bool UseItem()
    {
        Debug.Log("Use Knife");
        return true;
    }
}
