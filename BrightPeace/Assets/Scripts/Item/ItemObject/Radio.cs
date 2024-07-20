using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : Item
{
    public override bool UseItem()
    {
        Debug.Log("Use Radio");
        return true;
    }
}
