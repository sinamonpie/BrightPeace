using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{

    [SerializeField] private GameObject SlotsParent;
    [SerializeField] private TMP_Text alertText;
    private SlotBackGround[] slotsBg;
    private Slot[] slots;
    float wheelInput;
    int currentSlotNum;

    [SerializeField]
    private Slot currentSlot;       //  현재 슬롯
    void Start()
    {
        slots = SlotsParent.GetComponentsInChildren<Slot>();
        slotsBg = SlotsParent.GetComponentsInChildren<SlotBackGround>();
    }

    void Update()
    {
        wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyUp(KeyCode.Alpha1)) 
        {
            currentSlotNum = 0;
        }
        else if(Input.GetKeyUp(KeyCode.Alpha2)) 
        {
            currentSlotNum = 1;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            currentSlotNum = 2;
        }
        else if (wheelInput > 0)
        {
            currentSlotNum++;
            if (currentSlotNum > slots.Length - 1)
            {
                currentSlotNum = 0;
            }
        }
        else if (wheelInput < 0)
        {
            currentSlotNum--;
            if (currentSlotNum < 0)
            {
                currentSlotNum = slots.Length - 1;
            }
        }

        CurrentSlot(currentSlotNum);

        if (currentSlot != null)     // 아이템 사용
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                currentSlot.UseItemSlot();
                if (currentSlot.transform.GetComponent<Slot>().UsedItem())
                {
                    alertText.gameObject.SetActive(true);
                    alertText.text = currentSlot.item.itemName + " 을(를) 사용했습니다.";
                    currentSlot.ClearSlot();
                    Invoke("DisAlert", 2f);
                }
            }
            else if (Input.GetKeyUp(KeyCode.G)) 
            {
                currentSlot.DropItem();
            }
        }

    }

    void CurrentSlot(int index)
    {
        currentSlot = slots[index];

        for (int i = 0; i < slots.Length; i++)
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
                return true;
            }
        }
        return false;
    }
    public void DisAlert()
    {
        alertText.gameObject.SetActive(false);
    }

}
