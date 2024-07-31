using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private TMP_Text actionText;
    RaycastHit hitInfo;
    Ray ray;
    public Camera camera;
    public Inventory inventory;

    void Update()
    {
        CheckingPlayer();
    }   

    void CheckingPlayer()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
        if (inventory.currentSlot.item != null)
        {
            if (inventory.currentSlot.item.itemName == "Ä®")
            {
                if (Physics.Raycast(ray, out hitInfo, range, layerMask))
                {
                    if (hitInfo.transform.tag == "Player")
                    {
                        actionText.gameObject.SetActive(true);
                        actionText.text = "Ä® ÈÖµÎ¸£±â " + "<color=yellow>" + "Click!" + "</color>";

                        if (Input.GetMouseButtonDown(0))
                        {
                            Swing();
                        }
                    }
                    else
                    {
                        actionText.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                this.gameObject.SetActive(false);
            }

        }
        else
        {
            this.gameObject.SetActive(false);
        }

    }
    void Swing()
    {
        if (hitInfo.transform.tag == "Player")
        {
            if (hitInfo.transform.GetComponent<PlayerHp>() != null)
            {
                hitInfo.transform.GetComponent<PlayerHp>().currentHp -= 1;
                Debug.Log("´ë»ó ³²Àº Ã¼·Â : " + hitInfo.transform.GetComponent<PlayerHp>().currentHp.ToString());
            }
        }
        else
        {

        }

        inventory.currentSlot.ClearSlot();
        this.gameObject.SetActive(false);
    }
}
