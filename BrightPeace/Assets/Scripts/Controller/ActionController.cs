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
        actionText.text = "Open door " + "<color=yellow>"+ "E Key" + "</color>";
        DoorAction();
    }
    void InfoDisapper()
    {
        actionText.gameObject.SetActive(false);
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
                    Debug.Log(hitInfo.transform.GetComponent<Item>().itemData.itemName + " ȹ���߽��ϴ�.");
                    Destroy(hitInfo.transform.gameObject);
                }
                else
                {
                    Debug.Log("�κ��丮�� ���� á���ϴ�.");
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
                hitInfo.transform.GetComponent<DoorController>().DoorControl();
                Debug.Log(hitInfo);
                InfoDisapper();
            }
        }
    }
}
