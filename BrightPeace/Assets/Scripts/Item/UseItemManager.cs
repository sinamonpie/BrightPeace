using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using Photon.Pun;

/// <summary>
/// 아이템 사용/버리기와 관련된 기능 텍스트출력 등을 기재
/// </summary>
public class UseItemManager : MonoBehaviour
{
    [SerializeField] private TMP_Text alertText;

    public Inventory inventory;
    ItemActionManager actionManager;
    ActionController actionController;

    [Header("장착하고 있는 칼")]
    public GameObject knife;

    BoxCollider knifeCollider;
    Animator animator;

    [Header("공격 거리")]
    [SerializeField]
    float swingRange = 1.5f;

    [Header("공격 딜레이 시간")]
    [SerializeField]
    float swingDelay = 2.5f;
    float rate;
    bool isSwingReady;

    [SerializeField]
    RaycastHit hit;
    Ray ray;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        actionManager = FindObjectOfType<ItemActionManager>();
        actionController = FindObjectOfType<ActionController>();
        knife.gameObject.SetActive(false);

        animator = transform.GetComponent<Animator>();
        knifeCollider = knife.GetComponent<BoxCollider>();
        knifeCollider.enabled = false;
        rate = swingDelay;
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
                    ray = new Ray(transform.position + Vector3.up * 1f, transform.forward);
                    Debug.DrawRay(ray.origin, ray.direction * swingRange, Color.red);

                    knife.gameObject.SetActive(true);

                    rate += Time.deltaTime;
                    isSwingReady = rate > swingDelay;

                    if (Input.GetButtonDown("Fire1") && isSwingReady)
                    {
                        animator.SetTrigger("isSwing");
                        rate = 0;

                        if (Physics.Raycast(ray, out hit, swingRange))
                        {
                            if (hit.transform.CompareTag("Player"))
                            {
                                PlayerHp playerHp = hit.transform.GetComponent<PlayerHp>();
                                if(playerHp != null)
                                {
                                    playerHp.TakeDamage(1);
                                    Debug.Log("대상 남은 체력" + hit.transform.GetComponent<PlayerHp>().GetPlayerHp().ToString());
                                    inventory.currentSlot.ClearSlot();
                                    inventory.getKnife = false;
                                    knife.gameObject.SetActive(false);
                                }

                            }
                        }
                    }
                }
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
