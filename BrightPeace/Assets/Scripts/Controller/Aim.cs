using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Aim : MonoBehaviour
{
    public GameObject gunItem;
    public Image image;
    public Camera camera;
    public CinemachineVirtualCamera virtualCamera;
    private Cinemachine3rdPersonFollow _3rdPersonFollow;

    [SerializeField]
    float zoomedFOV = 30f;
    float normalFOV;
    float panDistance = 5f;

    private RaycastHit hitInfo;
    private Ray ray;

    [SerializeField]
    private LayerMask layerMask;

    private bool isZoomed = false;
    private bool isAimPlayer;
    void Start()
    {
        normalFOV = virtualCamera.m_Lens.FieldOfView;
        _3rdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    void Update()
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


    void Shoting()
    {
        if (isAimPlayer)
        {
            if (hitInfo.transform.GetComponent<PlayerHp>() != null)
            {
                hitInfo.transform.GetComponent<PlayerHp>().currentHp -= 1;
                Debug.Log("대상 남은 체력 : " + hitInfo.transform.GetComponent<PlayerHp>().currentHp.ToString());
            }
        }
        else { Debug.Log("감나빗"); }

        if (isZoomed)
        {
            isZoomed = !isZoomed;
            virtualCamera.m_Lens.FieldOfView = normalFOV;
            _3rdPersonFollow.ShoulderOffset.x -= panDistance;
            image.gameObject.SetActive(false);
        }

        this.gameObject.SetActive(false);
    }

    void Zooming()
    {
        isZoomed = !isZoomed;
        image.gameObject.SetActive(isZoomed);

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

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            if (hitInfo.transform.tag == "Player")
            {
                isAimPlayer = true;
                image.color = Color.red;
            }
            else
            {
                isAimPlayer = false;
                image.color = Color.white;
            }
        }
    }

}
