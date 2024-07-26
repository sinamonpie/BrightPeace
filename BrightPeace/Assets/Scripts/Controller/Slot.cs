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
    private ItemActionManager itemActionManager;
    void Start()
    {
        itemActionManager = FindObjectOfType<ItemActionManager>();
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

    public void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = color;
        itemUseSuccess = false;
    }
}
