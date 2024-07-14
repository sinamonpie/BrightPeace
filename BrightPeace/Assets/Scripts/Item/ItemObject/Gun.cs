using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item
{
    public override bool UseItem()
    {
        Debug.Log("Use Gun");
        return true;
    }
}
