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

    public Slot currentSlot;       //  현재 슬롯
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

        if (currentSlot.item != null)
        {
            if (currentSlot.item.itemType == ItemData.ItemType.Used)    //  소모품
            {
                if (Input.GetKeyUp(KeyCode.F))
                {
                    currentSlot.UseItemSlot();                          //   아이템 사용
                    if (currentSlot.transform.GetComponent<Slot>().UsedItem())
                    {
                        StartCoroutine(TextAlert());
                        alertText.text = currentSlot.item.itemName + " 을(를) 사용했습니다.";
                        currentSlot.ClearSlot();
                    }
                }
            }

            else if (currentSlot.item.itemType == ItemData.ItemType.Equip)
            {
                currentSlot.EquipItem();                                //  장비 
            }
            
            else if (currentSlot.item.itemType == ItemData.ItemType.Escape)
            {   
                                                                        // 탈출 장비
            }

            if (Input.GetKeyUp(KeyCode.G)) 
            {
                Vector3 PlayerPos = transform.parent.position;
                Vector3 PlayerFwd = transform.parent.forward;
                GameObject itemGo = Instantiate<GameObject>(currentSlot.item.itemPrefab);
                itemGo.transform.position = PlayerPos + PlayerFwd;
                Debug.Log("Drop " + currentSlot.item.itemName);
                currentSlot.ClearSlot();
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

    IEnumerator TextAlert()
    {
        alertText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        alertText.gameObject.SetActive(false);
    }

}
