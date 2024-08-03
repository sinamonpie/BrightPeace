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
    [Header("UI 텍스트")]
    [SerializeField] private TMP_Text alertText;

    Inventory inventory;
    ActionController actionController;

    GameObject knife;
    Animator animator;

    [Header("나이프 공격 거리")]
    [SerializeField]
    float swingRange = 1.5f;

    [Header("나이프 공격 딜레이 시간")]
    [SerializeField]
    float swingDelay = 2.5f;
    float rate;
    bool isSwingReady;

    [Header("열쇠 문 열리는 시간")]
    [SerializeField]
    float unlockTime = 2f;

    [Header("투시경 지속 시간")]
    [SerializeField]
    float wallHackTime = 3f;

    [SerializeField]
    RaycastHit hit;
    Ray ray;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        actionController = FindObjectOfType<ActionController>();
        knife = GameObject.FindWithTag("ItemHasPoint").gameObject;
        knife.gameObject.SetActive(false);

        animator = transform.GetComponent<Animator>();
        rate = swingDelay;
    }
    void Update()
    {

        if (inventory.currentSlot != null && inventory.currentSlot.item != null)
        {
            if (inventory.currentSlot.item.itemType == ItemType.Used)
            {
                // 아이템 줍기랑 사용 중복 제한
                if (Input.GetKeyDown(KeyCode.E) && !actionController.isRayItem)
                {
                    switch (inventory.currentSlot.item.itemName)
                    {
                        case "열쇠":
                        {
                            // 잠기지 않은 문은 아이템 사용 불가
                            if (!actionController.canDoor)
                            goto exit;

                            if (!GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().IsLockDoor())
                            {
                                if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().CanDoorAction(unlockTime))
                                {
                                    StartCoroutine(UseKey(unlockTime));
                                }
                            }
                            break;
                        }

                        case "구급약":
                        {
                            // 체력이 2면 사용 불가
                            int currnetPlayerHp = transform.GetComponent<PlayerHp>().GetPlayerHp();
                            if (currnetPlayerHp > 1)
                            {
                                goto exit;
                            }
                            // 자신 회복
                            transform.GetComponent<PlayerHp>().Heal(1);
                            break;
                        }

                        case "투시경":
                        {                   
                            // 단, 캐비넷에 들어가있는 플레이어는 감지되지 않는다.
                            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrayScreen>().ApplyGrayScreen(wallHackTime);
                            GameObject.FindAnyObjectByType<WallHacker>().ApplyWallHack(wallHackTime);
                            break;
                        }
                    }
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
            // 아이템 버리기
            if (Input.GetKeyUp(KeyCode.G)) 
            {
                Vector3 PlayerPos = transform.position;
                Vector3 PlayerFwd = transform.forward;
                GameObject itemGo = Instantiate<GameObject>(inventory.currentSlot.item.itemPrefab);
                itemGo.transform.position = PlayerPos + PlayerFwd;
                StartCoroutine(TextAlert());
                alertText.text = inventory.currentSlot.item.itemName + " 을(를) 떨어뜨렸습니다.";
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

    IEnumerator UseKey(float time)               // 2초후 문 열림
    {
        yield return new WaitForSeconds(time);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().UnlockDoor();
    }

}
