using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ActionController : MonoBehaviour
{
    [Header("상호작용 거리")]
    [SerializeField]
    private float range;
    private RaycastHit hitInfo;
    private Ray ray;
    private bool isInvenFull;
    private GameObject currentLockDoor;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private TMP_Text actionText;
    [SerializeField] private TMP_Text alertText;

    Inventory inventory;
    public bool isRayItem;
    public bool canDoor = false;
    public GameObject player;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        CheckInteraction();
    }

    void CheckInteraction()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.cyan);

        if (Physics.Raycast(ray, out hitInfo, range, layerMask))
        {

            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
                isRayItem = true;
            }
            
            if(hitInfo.transform.tag == "Door")
            {
                canDoor = true;
                DoorInfoAppear();
            }

            if(hitInfo.transform.tag == "Ending")
            {
                canDoor = true;
                EndigInfoAppear();
            }

            if (hitInfo.transform.tag == "Cabinet")
            {
                canDoor = true;
                HideCabinetInfoAppear();
            }

        }
        else
        {
            InfoDisapper();
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
                    Debug.Log("획득하기 " + hitInfo.transform.GetComponent<ItemPickUp>().Item.itemName);
                    Destroy(hitInfo.transform.gameObject);
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
                    hitInfo.transform.GetComponent<DoorController>().DoorControl();
                    Debug.Log(hitInfo);
                    InfoDisapper();
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
        if(Input.GetKeyDown(KeyCode.E))
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

        // 자식 오브젝트들에 대해서 재귀적으로 레이어를 변경
        foreach (Transform child in player.transform)
        {
            if (child != null)
            {
                ChangePlayerLayer(child.gameObject, toLayer);
            }
        }
    }
}
