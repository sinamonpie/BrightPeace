using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    public ItemData item;           // »πµÊ«— æ∆¿Ã≈€
    public Image itemImage;         // »πµÊ«— æ∆¿Ã≈€ ¿ÃπÃ¡ˆ

    void Start()
    {
        itemImage = transform.GetComponent<Image>();
    }

    public void AddItem(ItemData item)
    {
        this.item = item;
        itemImage.sprite = item.itemImage;
    }

    public void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
    }
}
