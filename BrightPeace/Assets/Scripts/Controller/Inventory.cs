using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{

    [SerializeField]
    private GameObject SlotsParent;
    [SerializeField]
    private Slot[] slots;

    void Start()
    {
        slots = SlotsParent.GetComponentsInChildren<Slot>();
    }

    public bool AddItem(ItemData item)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item);
                return true;
            }
        }
        return false;
    }

}
