using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UseItemManager : MonoBehaviour
{
    [SerializeField] private TMP_Text alertText;

    Inventory inventory;
    ItemActionManager actionManager;
    ActionController actionController;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        actionManager = FindObjectOfType<ItemActionManager>();
        actionController = FindObjectOfType<ActionController>();
    }
    void Update()
    {

        if (inventory != null && inventory.currentSlot != null)
        {
            if (inventory.currentSlot.item != null)
            {
                if (inventory.currentSlot.item.itemType == ItemType.Used)    //  소모품
                {
                    if (Input.GetKeyUp(KeyCode.F))
                    {
                        actionManager.UseItem(inventory.currentSlot.item);      // 아이템 사용
                        StartCoroutine(TextAlert());
                        alertText.text = inventory.currentSlot.item.itemName + " 을(를) 사용했습니다.";
                        inventory.currentSlot.ClearSlot();
                    }
                }

                else if (inventory.currentSlot.item.itemType == ItemType.Equip)
                {
                    actionManager.UseItem(inventory.currentSlot.item);
                }

                if (Input.GetKeyUp(KeyCode.G))
                {
                    Vector3 PlayerPos = transform.position;
                    Vector3 PlayerFwd = transform.forward;
                    GameObject itemGo = Instantiate<GameObject>(inventory.currentSlot.item.itemPrefab);
                    itemGo.transform.position = PlayerPos + PlayerFwd;
                    Debug.Log("Drop " + inventory.currentSlot.item.itemName);
                    inventory.currentSlot.ClearSlot();
                }
            }
        }

    }

    IEnumerator TextAlert()
    {
        alertText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        alertText.gameObject.SetActive(false);
    }
}
