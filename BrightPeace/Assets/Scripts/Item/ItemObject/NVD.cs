using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NVD : Item
{
    public override bool UseItem()
    {
        Debug.Log("Use NVD");
        return true;
    }
}
