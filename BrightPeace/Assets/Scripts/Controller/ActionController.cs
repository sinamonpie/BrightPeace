using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;
    private RaycastHit hitInfo;
    private Ray ray;
    private bool isInvenFull;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private TMP_Text actionText;

    public Inventory inventory;

    private bool canDoor = false;

    void Update()
    {
        CheckInteraction();
    }

    void CheckInteraction()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo, range, layerMask))
        {

            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
            
            if(hitInfo.transform.tag == "Door")
            {
                canDoor = true;
                DoorInfoAppear();
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
        actionText.text = hitInfo.transform.GetComponent<Item>().itemData.itemName + " Get " + "<color=yellow>" + "E Key" + "</color>";
        PickupAction();
    }

    void DoorInfoAppear()
    {
        actionText.gameObject.SetActive(true);
        if (hitInfo.transform.GetComponent<DoorController>().isClose)
        {
            actionText.text = "Open door " + "<color=yellow>" + "E Key" + "</color>";
        }
        else 
        {
            actionText.text = "Close door " + "<color=yellow>" + "E Key" + "</color>";
        }
        DoorAction();
    }
    void InfoDisapper()
    {
        actionText.gameObject.SetActive(false);
        canDoor = false;
    }

    void PickupAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hitInfo.transform != null)
            {
                isInvenFull = inventory.AddItem(hitInfo.transform.GetComponent<Item>().itemData);
                if (isInvenFull)
                {
                    Debug.Log("Get " + hitInfo.transform.GetComponent<Item>().itemData.itemName);
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
                    // 문 잠겨있음 알림띄우기
                }
            }
        }
    }

    public bool CanDoorAction()
    {
        return canDoor;
    }

    public void UnlockDoor()
    {
        hitInfo.transform.GetComponent<DoorController>().UnlockDoor();
    }
}
