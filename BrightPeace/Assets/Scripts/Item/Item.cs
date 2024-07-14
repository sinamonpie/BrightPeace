using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;

    public virtual bool UseItem()
    {
        return true;
    }
}
