using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    protected float gravity = -9.81f;
    protected Vector3 moveDirection;

    public Transform cameraTransform;
    protected CharacterController characterController;

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
}