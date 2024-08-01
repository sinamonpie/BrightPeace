using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이템 사용/버리기와 관련된 기능 텍스트출력 등을 기재
/// </summary>
public class UseItemManager : MonoBehaviour
{
    [SerializeField] private TMP_Text alertText;

    Inventory inventory;
    ItemActionManager actionManager;
    ActionController actionController;
    [Header("장착하고 있는 칼")]
    public GameObject setKnife;
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        actionManager = FindObjectOfType<ItemActionManager>();
        actionController = FindObjectOfType<ActionController>();
        setKnife.gameObject.SetActive(false);
    }
    void Update()
    {

        if (inventory.currentSlot != null && inventory.currentSlot.item != null)
        {
            if (inventory.currentSlot.item.itemType == ItemType.Used)    //  소모품
            {
                if (Input.GetKeyDown(KeyCode.E) && !actionController.isRayItem) // 아이템 줍기랑 사용 중복 제한
                {
                    switch (inventory.currentSlot.item.itemName)            // 아이템 사용 조건
                    {
                        case "열쇠":
                        {
                            if (!actionController.canDoor)                                  // 키 사용 불가능
                            goto exit;
                        }
                        break;
                    }
                    actionManager.UseItem(inventory.currentSlot.item);
                    StartCoroutine(TextAlert());
                    alertText.text = inventory.currentSlot.item.itemName + " 을(를) 사용했습니다.";
                    inventory.currentSlot.ClearSlot();
                }
            exit:;
               
            }

            else if (inventory.currentSlot.item.itemType == ItemType.Equip)
            {
                if(inventory.currentSlot.item.itemName == "칼")
                {
                    setKnife.gameObject.SetActive(true);
                }
                actionManager.UseItem(inventory.currentSlot.item);
            }

            if (Input.GetKeyUp(KeyCode.G))  // 아이템 버리기
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

    IEnumerator TextAlert()
    {
        alertText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        alertText.gameObject.SetActive(false);
    }
}
