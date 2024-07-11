using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InteractionController : MonoBehaviour
{
    [SerializeField]
    private float range;                //  상호작용 거리
    private RaycastHit hitInfo;         //  충돌체 정보
    private Ray ray;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private TMP_Text actionText;
    [SerializeField] private Inventory inventory;
    void Update()
    {
        CheckInteraction();
    }

    void CheckInteraction()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform.tag == "Item")
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
                Debug.Log(hitInfo.transform.GetComponent<Item>().itemData.itemName + " 획득했습니다.");
/*                inventory.GetItem(hitInfo.transform.GetComponent<Item>().itemData);*/
                Destroy(hitInfo.transform.gameObject);
                InfoDisapper();
            }
        }
    }
}
