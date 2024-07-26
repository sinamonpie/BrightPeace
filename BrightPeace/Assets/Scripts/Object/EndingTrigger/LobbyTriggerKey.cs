using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Older;
public class LobbyTriggerKey : Item
{
    public override bool UseItem()
    {
        Debug.Log("escapeKey");
        return true;
    }
}
