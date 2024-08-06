using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    protected float gravity = -9.81f;
    protected Vector3 moveDirection;

    public Transform cameraTransform;
    protected CharacterController characterController;

    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }
}