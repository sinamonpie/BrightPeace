using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InteractionController : MonoBehaviour
{
    [SerializeField]
    private float range;                //  상호작용 거리
    private bool activated = false;     //  상호작용 여부
    private RaycastHit hitInfo;         //  충돌체 정보

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private TMP_Text actionText;
    void Update()
    {
        CheckInteraction();
        PickupAction();
    }

    void CheckInteraction()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
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
        activated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<Item>().itemData.itemName + "Get " + "<color=yellow>" + "E Key" + "</color>";
    }

    void DoorInfoAppear()
    {
        activated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = "Get " + "<color=yellow>"+ "E Key" + "</color>";
    }
    void InfoDisapper()
    {
        activated = false;
        actionText.gameObject.SetActive(false);
    }

    void PickupAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("2");
            if (activated)
            {
                Debug.Log("3");
                if (hitInfo.transform != null)
                {
                    Debug.Log("4");
                    Debug.Log(hitInfo.transform.GetComponent<Item>().itemData.itemName + " 획득했습니다.");
                    Destroy(hitInfo.transform.gameObject);
                    InfoDisapper();
                }
            }
        }
    }
}
