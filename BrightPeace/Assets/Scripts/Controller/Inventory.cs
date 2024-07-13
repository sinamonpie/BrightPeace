using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{

    [SerializeField] 
    private GameObject SlotsParent;
    private SlotBackGround[] slotsBg;
    private Slot[] slots;

    [SerializeField]
    private Slot currentSlot;
    private int slotsCount;
    void Start()
    {
        slots = SlotsParent.GetComponentsInChildren<Slot>();
        slotsBg = SlotsParent.GetComponentsInChildren<SlotBackGround>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) 
        {
            CurrentSlot(0);
        }
        else if(Input.GetKeyUp(KeyCode.Alpha2)) 
        {
            if(slotsCount > 1)
            {
                CurrentSlot(1);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            if(slotsCount > 2)
            {
                CurrentSlot(2);
            }
        }


        if(currentSlot != null)
        {
            
            //  아이템 사용 함수
        }
    }

    void CurrentSlot(int index)
    {
        if (slots[index].item != null)
        {
            currentSlot = slots[index];
        }

        for (int i = 0; i < slotsCount; i++)
        {
            if (i == index)
            {
                slotsBg[i].SetSlot();
                continue;
            }
            slotsBg[i].DisSlot();

        }
    }
    
    public bool AddItem(ItemData item)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item);
                slotsCount++;
                return true;
            }
        }
        return false;
    }

}
