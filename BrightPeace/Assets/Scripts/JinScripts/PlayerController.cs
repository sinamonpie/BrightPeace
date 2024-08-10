using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 2.0f;

    [SerializeField]
    public float SprintSpeed = 5.335f;

    protected float gravity = -9.81f;
    protected Vector3 moveDirection;

    public Transform cameraTransform;
    protected CharacterController characterController;

    protected bool isMove = true;

    [SerializeField]
    protected PhotonView pv;

    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }

    public void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public void EnableMove()
    {
        isMove = true;
    }

    public void UnEnableMove()
    {
        isMove = false;
    }
}