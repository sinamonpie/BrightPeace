using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public GameObject SlotsParent;
    public TMP_Text alertText;
    [SerializeField] private SlotBackGround[] slotsBg;
    [SerializeField] private Slot[] slots;
    float wheelInput;
    int currentSlotNum;
    int knifeSlotNum;
    public bool getKnife;
    public bool settingInventory;

    //  ÇöÀç ½½·Ô
    public Slot currentSlot;
    private PhotonView pv;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        settingInventory = false;

        if(pv != null && SlotsParent == null && alertText == null)
        {
            if(pv.IsMine)
            {
                SettingInventory();
            }
        }
    }

    void Update()
    {
        if (settingInventory)
        {
            wheelInput = Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                currentSlotNum = 0;
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
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
        }
    }

    public void SettingInventory()
    {
        SlotsParent = InGameManager.Instance.SlotParents;
        alertText = InGameManager.Instance.alertText;
        slots = SlotsParent.GetComponentsInChildren<Slot>();
        slotsBg = SlotsParent.GetComponentsInChildren<SlotBackGround>();
        settingInventory = true;
    }

    void CurrentSlot(int index)
    {
        if (getKnife)
        {
            currentSlot = slots[knifeSlotNum];
        }
        else
        {
            currentSlot = slots[index];
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (!getKnife)
            {
                if (i == index)
                {
                    slotsBg[i].SetSlot();
                }
                else
                {
                    slotsBg[i].DisSlot();
                }
            }
            else
            {
                if (i == knifeSlotNum)
                {
                    slotsBg[i].SetSlot();
                }
                else
                {
                    slotsBg[i].DisSlot();
                }
            }
        }
    }
    public bool AddItem(ItemData item)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item);
                if(item.itemName == "Ä®")
                {
                    getKnife = true;
                    knifeSlotNum = i;
                }
                return true;
            }
        }
        return false;
    }

}
