using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    public ItemData item;           // »πµÊ«— æ∆¿Ã≈€
    public Image itemImage;         // »πµÊ«— æ∆¿Ã≈€ ¿ÃπÃ¡ˆ
    private Color color;
    private bool itemUseSuccess;
    void Start()
    {
        itemImage = transform.GetComponent<Image>();
        color = itemImage.color;
    }
    public void SetColor(float r, float g, float b)
    {
        Color color = new Color(r, g, b);
        itemImage.color = color;
    }
    public void AddItem(ItemData item)
    {
        this.item = item;
        itemImage.sprite = item.itemImage;
        SetColor(255f, 255f, 255f);
    }

    public void DropItem()
    {
        Vector3 PlayerPos = GameObject.FindWithTag("Player").transform.position;
        Vector3 PlayerFwd = GameObject.FindWithTag("Player").transform.forward;
        GameObject itemGo = Instantiate<GameObject>(item.itemPrefab);
        itemGo.transform.position = PlayerPos + PlayerFwd;
        Debug.Log("Drop " + item.itemName);
        ClearSlot();
    }

    public void UseItemSlot()
    {
        if(item != null) 
        {
            itemUseSuccess = item.itemPrefab.transform.GetComponentInChildren<Item>().UseItem();
        }
    }

    public bool UsedItem()
    {
        return itemUseSuccess;
    }

    public void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = color;
    }
}
