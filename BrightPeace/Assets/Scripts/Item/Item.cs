using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;

    public virtual bool UseItem()
    {
        return true;
    }

    public virtual void Equip()
    {

    }

    public virtual void UnEquip()
    {

    }
}
