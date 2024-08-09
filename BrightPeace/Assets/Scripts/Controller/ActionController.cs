using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Photon.Pun;

public class ActionController : MonoBehaviourPun
{
    [Header("상호작용 거리")]
    [SerializeField]
    private float range;
    public RaycastHit hitInfo;
    private Ray ray;
    private bool isInvenFull;
    private GameObject currentLockDoor;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] public TMP_Text actionText;
    [SerializeField] private TMP_Text alertText;

    [SerializeField] Inventory inventory;
    [SerializeField] GameObject player;

    public bool isRayItem;
    public bool canDoor = false;
    private bool isSetting = false;
    private bool isUseValve = false;

    void Update()
    {
        if (isSetting)
        {
            CheckInteraction();
        }
    }

    void CheckInteraction()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.cyan);

        if (Physics.Raycast(ray, out hitInfo, range, layerMask))
        {

            if (hitInfo.transform.tag == "Item" && !PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                ItemInfoAppear();
                isRayItem = true;
            }

            if (hitInfo.transform.tag == "Door")
            {
                canDoor = true;
                DoorInfoAppear();
            }

            if (hitInfo.transform.tag == "Ending" && !PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                canDoor = true;
                EndigInfoAppear();
            }

            if (hitInfo.transform.tag == "Cabinet" && !PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                canDoor = true;
                HideCabinetInfoAppear();
            }

            if (hitInfo.transform.tag == "Tank")
            {
                if (hitInfo.transform.GetComponent<ValveTank>().GetValve())
                {
                    if (!isUseValve)
                    {
                        actionText.gameObject.SetActive(true);
                        actionText.text = "밸브 돌리기 " + "<color=yellow>" + "E Key" + "</color>";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            UseValve();
                        }
                    }
                    else
                    {
                        actionText.gameObject.SetActive(false);
                    }
                }
                else
                {
                    alertText.gameObject.SetActive(true);
                    alertText.text = "발전기를 사용하려면 밸브 아이템이 필요합니다. ";
                }

            }

            if (hitInfo.transform.tag == "FuseBox" && !PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                canDoor = true;
                HideCabinetInfoAppear();
            }

            else
            {
                InfoDisapper();
            }
        }

    }
    void ItemInfoAppear()
    {
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().Item.itemName + " 획득하기 " + "<color=yellow>" + "E Key" + "</color>";
        PickupAction();
    }

    void DoorInfoAppear()
    {
        actionText.gameObject.SetActive(true);

        if (hitInfo.transform.GetComponent<DoorController>().isClose)
        {
            actionText.text = "문 열기 " + "<color=yellow>" + "E키" + "</color>";
        }
        else
        {
            actionText.text = "문 닫기 " + "<color=yellow>" + "E키" + "</color>";
        }
        DoorAction();
    }
    void InfoDisapper()
    {
        actionText.gameObject.SetActive(false);
        alertText.gameObject.SetActive(false);
        canDoor = false;
        isRayItem = false;
    }

    void PickupAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hitInfo.transform != null)
            {
                isInvenFull = inventory.AddItem(hitInfo.transform.GetComponent<ItemPickUp>().Item);
                if (isInvenFull)
                {
                    // 아이템 주울때 사운드 추가
                    Debug.Log("획득하기 " + hitInfo.transform.GetComponent<ItemPickUp>().Item.itemName);
                    GameObject item = hitInfo.transform.gameObject;
                    item.GetComponent<ItemPickUp>().TakeItem();
                }
                else
                {
                    Debug.Log("Full Inventory");
                }
                InfoDisapper();
            }
        }
    }

    void DoorAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.GetComponent<DoorController>().UseableDoor())
                {
                    // 문 열리는 사운드 추가
                    hitInfo.transform.GetComponent<DoorController>().DoorControl();
                    Debug.Log(hitInfo);
                    InfoDisapper();
                }
                else
                {
                    // 문 잠긴 사운드 추가
                    alertText.gameObject.SetActive(true);
                    alertText.text = "문이 잠겼습니다.";
                    Invoke("DisAlert", 1f);
                }
            }
        }
    }
    public bool IsLockDoor()
    {
        if (hitInfo.transform.GetComponent<DoorController>().UseableDoor())
        {
            return true;        // 열린문
        }
        return false;           // 잠긴문
    }

    public bool CanDoorAction()
    {
        return canDoor;
    }
    public bool CanDoorAction(float time)
    {
        hitInfo.transform.GetComponentInChildren<DoorUseKeyUI>().DoorUI(time);
        currentLockDoor = hitInfo.transform.gameObject;
        return canDoor;
    }

    public void UnlockDoor()
    {
        currentLockDoor.GetComponent<DoorController>().UnlockDoor();
        currentLockDoor = null;
    }

    public void DisAlert()
    {
        alertText.gameObject.SetActive(false);
    }

    public void EndingAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.GetComponent<EscapeEnding>().EndigTriiger())
                {
                    Debug.Log("엔딩조건 충족 / 엔딩씬 보여주기");
                }
                else
                {
                    alertText.gameObject.SetActive(true);
                    alertText.text = "문이 잠겼습니다.";
                    Invoke("DisAlert", 1f);
                }
            }
        }
    }

    public void EndigInfoAppear()
    {
        actionText.gameObject.SetActive(true);
        actionText.text = "<color=yellow>" + " 탈출하기 " + "E Key" + "</color>";
        EndingAction();
    }

    public void HideCabinetAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hitInfo.transform != null)
            {
                hitInfo.transform.GetComponent<CabinetController>().CabinetControl(player);
                if (hitInfo.transform.GetComponent<CabinetController>().HideInCabinet())
                {
                    // 캐비넷 들어가는 사운드 추가
                    // 플레이어 투시경에 안보이게 하는거 임시용
                    player.SetActive(false);
                    player.GetComponent<PlayerState>().PlayerInCabinet();

                    // 7 = hiddenCharactor Layer
                    ChangePlayerLayer(player, 7);
                }
                else
                {
                    player.SetActive(true);
                    player.GetComponent<PlayerState>().PlayerInCabinet();

                    // 8 = Charactor Layer
                    ChangePlayerLayer(player, 8);
                }
            }
        }
    }

    public void HideCabinetInfoAppear()
    {
        actionText.gameObject.SetActive(true);
        if (hitInfo.transform.GetComponent<CabinetController>().HideInCabinet())
        {
            actionText.text = "캐비넷에서 나오기 " + "<color=yellow>" + "E키" + "</color>";
        }
        else
        {
            actionText.text = "캐비넷에 숨기 " + "<color=yellow>" + "E키" + "</color>";
        }
        HideCabinetAction();
    }

    void ChangePlayerLayer(GameObject player, int toLayer)
    {
        player.layer = toLayer;

        foreach (Transform child in player.transform)
        {
            if (child != null)
            {
                ChangePlayerLayer(child.gameObject, toLayer);
            }
        }
    }

    public void SetPlayer()
    {
        player = transform.root.gameObject;
        inventory = null;

        if (!PhotonNetwork.IsMasterClient && photonView.IsMine)
        {
            inventory = player.GetComponent<Inventory>();
        }
        isSetting = true;
    }

    void UseValve()
    {
        GameObject tank = hitInfo.transform.gameObject;
        tank.GetComponent<ValveTank>().OpenTheGate();
        isUseValve = true;
    }

    IEnumerator TextAlert()
    {
        alertText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        alertText.gameObject.SetActive(false);
    }
    }
