using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using Unity.VisualScripting;

/// <summary>
/// 아이템 사용/버리기와 관련된 기능 텍스트출력 등을 기재
/// 아이템 처리 -> 마스터 클라이언트
/// </summary>
public class UseItemManager : MonoBehaviourPun
{
    TMP_Text alertText;
    Inventory inventory;
    ActionController actionController;

    [SerializeField] Camera mainCamera;
    [SerializeField] SensorCamera sensorCamera;

    GameObject knife;
    Animator animator;
    PhotonView pv;

    [Header("나이프 공격 거리")]
    [SerializeField]
    float swingRange = 1.5f;

    [Header("나이프 공격 딜레이 시간")]
    [SerializeField]
    float swingDelay = 2.5f;
    float rate;
    bool isSwingReady;

    [Header("나이프 스턴 시간")]
    [SerializeField]
    float stunTime = 2f;

    [Header("회복 아이템 횟수 제한")]
    [SerializeField]
    int medikitRate = 2;
    int medikitUseRate;

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
        pv = GetComponent<PhotonView>(); 
        
        if(pv.IsMine)
        {
            inventory = FindObjectOfType<Inventory>();
            alertText = inventory.alertText;
            actionController = FindObjectOfType<ActionController>();
            knife = GameObject.FindWithTag("ItemHasPoint").gameObject;
            knife.gameObject.SetActive(false);
            mainCamera = GetComponentInChildren<Camera>();
            sensorCamera = mainCamera.gameObject.GetComponentInChildren<SensorCamera>();

            animator = transform.GetComponent<Animator>();
            rate = swingDelay;
            medikitUseRate = 0;
        }

    }
    void Update()
    {
        if (pv.IsMine)
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

                                    if (!mainCamera.gameObject.GetComponent<ActionController>().IsLockDoor())
                                    {
                                        if (mainCamera.gameObject.GetComponent<ActionController>().CanDoorAction(unlockTime))
                                        {
                                            StartCoroutine(UseKey(unlockTime));
                                        }
                                    }
                                    break;
                                }

                            case "구급약":
                                {
                                    if (medikitRate < medikitUseRate)
                                    {
                                        goto exit;
                                    }

                                    int currnetPlayerHp = transform.GetComponent<PlayerState>().GetPlayerHp();

                                    if (currnetPlayerHp > 1)
                                    {
                                        goto exit;
                                    }

                                    transform.GetComponent<PlayerState>().Heal(1);
                                    medikitUseRate++;
                                    break;
                                }

                            case "투시경":
                                {
                                    // 단, 캐비넷에 들어가있는 플레이어는 감지되지 않는다.
                                    mainCamera.GetComponentInChildren<GrayScreen>().ApplyGrayScreen(wallHackTime);
                                    StartCoroutine(WallHack(wallHackTime));
                                    break;
                                }

                            case "드라이버":
                                {
                                    if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().CanDoorAction())
                                    {
                                        GameObject.FindGameObjectWithTag("Ending").GetComponent<EscapeEnding>().Driver_Trigger();
                                        GameObject.FindGameObjectWithTag("Ending").GetComponent<EscapeEnding>().EndigTriggerCheck();
                                        Debug.Log("Use Dirver");

                                    }
                                    break;
                                }

                            case "쇠지렛대":
                                {
                                    if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().CanDoorAction())
                                    {
                                        GameObject.FindGameObjectWithTag("Ending").GetComponent<EscapeEnding>().CrowBar_Trigger();
                                        GameObject.FindGameObjectWithTag("Ending").GetComponent<EscapeEnding>().EndigTriggerCheck();
                                        Debug.Log("Use CrowBar");
                                    }
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
                    if (inventory.currentSlot.item.itemName == "칼")
                    {
                        ray = new Ray(transform.position + Vector3.up * 1f, transform.forward);
                        Debug.DrawRay(ray.origin, ray.direction * swingRange, Color.red);

                        if (!knife.activeSelf)
                        {
                            pv.RPC("ShowKnife", RpcTarget.All, true);
                        }

                        rate += Time.deltaTime;
                        isSwingReady = rate > swingDelay;

                        if (Input.GetButtonDown("Fire1") && isSwingReady)
                        {
                            // 나이프 휘두르는 사운드 추가
                            animator.SetTrigger("isSwing");
                            rate = 0;

                            if (Physics.Raycast(ray, out hit, swingRange))
                            {
                                if (hit.transform.CompareTag("Player"))
                                {
                                    PlayerState playerState = hit.transform.GetComponent<PlayerState>();
                                    
                                    // 다른 플레이어가 맞았으면 
                                    if (playerState != null)
                                    {
                                        pv.RPC("AttackPlayer", RpcTarget.All, hit.transform.GetComponent<PhotonView>().ViewID);

                                        inventory.currentSlot.ClearSlot();
                                        inventory.getKnife = false;
                                        pv.RPC("ShowKnife", RpcTarget.All, false);
                                    }

                                }
                            }
                        }


                    }

                }

                else if (inventory.currentSlot.item.itemType == ItemType.Escape)
                {
                    // 다른 아이템 줍기 중복 제한
                    if (!actionController.isRayItem)
                    {
                        if (inventory.currentSlot.item.itemName == "퓨즈")
                        {

                        }
                        else if (inventory.currentSlot.item.itemName == "락픽")
                        {

                        }
                        else if (inventory.currentSlot.item.itemName == "밸브")
                        {

                        }

                    }

                }
                // 아이템 버리기
                if (Input.GetKeyUp(KeyCode.G))
                {
                    Vector3 PlayerPos = transform.position;
                    Vector3 PlayerFwd = transform.forward;
                    string itemName = inventory.currentSlot.item.itemPrefab.name;

                    StartCoroutine(TextAlert());
                    alertText.text = inventory.currentSlot.item.itemName + " 을(를) 떨어뜨렸습니다.";

                    if (inventory.currentSlot.item.itemName == "칼")
                        inventory.getKnife = false;

                    pv.RPC("RPC_DropItem", RpcTarget.MasterClient, PlayerPos + PlayerFwd, Quaternion.identity, itemName);

                    inventory.currentSlot.ClearSlot();
                }
            }
        }
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    IEnumerator WallHack(float wallHackTime)
    {
        sensorCamera.SetCamera(true);
        GameObject player = transform.gameObject;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject otherPlayer in players)
        {
            if (otherPlayer != player)
            {
                PlayerState state = otherPlayer.GetComponent<PlayerState>();
                if (state != null && !state.IsInCabinet())
                {
                    otherPlayer.GetComponentInChildren<PlayerRenderer>().ApplyHighlight(wallHackTime);
                }
            }
        }

        yield return new WaitForSeconds(wallHackTime);

        sensorCamera.SetCamera(false);

    }

    IEnumerator TextAlert()
    {
        alertText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        alertText.gameObject.SetActive(false);
    }

    IEnumerator UseKey(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().UnlockDoor();
    }

    [PunRPC]
    void RPC_DropItem(Vector3 position, Quaternion rotation, string itemName)
    {
        PhotonNetwork.Instantiate(itemName, position, rotation);
    }

    [PunRPC]
    void ShowKnife(bool show)
    {
        if (pv.IsMine)
        {
            knife.gameObject.SetActive(show);
        }
    }

    [PunRPC]
    void AttackPlayer(int targetViewID)
    {
        PhotonView targetView = PhotonView.Find(targetViewID);
        if(targetView != null)
        {
            // 조건 추가 경비원일때는 스턴 2초
            if (targetView.Owner.IsMasterClient)
            {
                targetView.RPC("RPC_Stun", RpcTarget.MasterClient, stunTime);
            }
            else
            {
                PlayerState playerHp = targetView.GetComponent<PlayerState>();
                if (playerHp != null)
                {
                    playerHp.TakeDamage(1);
                    Debug.Log("대상 남은 체력 : " + playerHp.GetPlayerHp().ToString());
                }
            }
        }
    }
}
