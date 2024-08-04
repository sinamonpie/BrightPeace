using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed = 2.0f;
    protected float gravity = -9.81f;
    protected Vector3 moveDirection;

    [SerializeField]
    protected Transform cameraTransform;
    protected CharacterController characterController;

    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }
}