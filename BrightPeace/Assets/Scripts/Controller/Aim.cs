using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class Aim : MonoBehaviour
{
    public Image zoomCrosshair;
    public Camera camera;
    public CinemachineVirtualCamera virtualCamera;
    private Cinemachine3rdPersonFollow _3rdPersonFollow;

    [SerializeField]
    float zoomedFOV = 30f;
    float normalFOV;
    float panDistance = 5f;

    private RaycastHit hit;
    private Ray ray;

    [SerializeField]
    private LayerMask layerMask;

    private bool isZoomed = false;
    private bool isAimPlayer;

    Inventory inventory;
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        normalFOV = virtualCamera.m_Lens.FieldOfView;
        _3rdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    void Update()
    {
        if (inventory.currentSlot.item != null)
        {
            if (inventory.currentSlot.item.itemName == "ÃÑ")
            {
                Aimming();

                if (Input.GetMouseButtonDown(0))
                {
                    Shoting();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Zooming();
                }
            }
            else
            {
                if (isZoomed)
                {
                    isZoomed = !isZoomed;
                    virtualCamera.m_Lens.FieldOfView = normalFOV;
                    _3rdPersonFollow.ShoulderOffset.x -= panDistance;
                    zoomCrosshair.gameObject.SetActive(false);
                }
                this.gameObject.SetActive(false);
            }

        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    void Shoting()
    {
        if (isAimPlayer)
        {
            if (hit.transform.GetComponent<PlayerState>() != null)
            {
                hit.transform.GetComponent<PlayerState>().TakeDamage(1, UserRole.Mental);
                Debug.Log("´ë»ó ³²Àº Ã¼·Â : " + hit.transform.GetComponent<PlayerState>().GetPlayerHp().ToString());
            }
        }
        else { Debug.Log("°¨³ªºø"); }

        if (isZoomed)
        {
            isZoomed = !isZoomed;
            virtualCamera.m_Lens.FieldOfView = normalFOV;
            _3rdPersonFollow.ShoulderOffset.x -= panDistance;
            zoomCrosshair.gameObject.SetActive(false);
        }

        inventory.currentSlot.ClearSlot();
        this.gameObject.SetActive(false);
    }

    void Zooming()
    {
        isZoomed = !isZoomed;
        zoomCrosshair.gameObject.SetActive(isZoomed);

        if (isZoomed)
        {
            virtualCamera.m_Lens.FieldOfView = zoomedFOV;
            _3rdPersonFollow.ShoulderOffset.x += panDistance;
        }
        else
        {
            virtualCamera.m_Lens.FieldOfView = normalFOV;
            _3rdPersonFollow.ShoulderOffset.x -= panDistance;
        }
    }

    void Aimming()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.tag == "Player" && hit.transform != null)
            {
                isAimPlayer = true;
                zoomCrosshair.color = Color.red;
            }
            else
            {
                isAimPlayer = false;
                zoomCrosshair.color = Color.white;
            }
        }
    }

}
