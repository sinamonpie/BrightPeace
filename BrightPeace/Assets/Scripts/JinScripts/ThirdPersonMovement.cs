using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : PlayerController
{
    public float rotationSpeed = 100;
    private Vector3 rotation;
    public Vector3 cameraOffset = new Vector3(0, 2, -3);

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        MoveTo(new Vector3(x, 0, z));

        float mouseX = Input.GetAxis("Mouse X");

        rotation = new Vector3(0, mouseX * rotationSpeed * Time.deltaTime, 0);
        transform.Rotate(rotation);

        UpdateCameraPosition();
    }

    public void MoveTo(Vector3 direction)
    {
        Vector3 moveDir = transform.rotation * direction;
        moveDirection = new Vector3(moveDir.x, moveDirection.y, moveDir.z);

        if (!characterController.isGrounded)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void UpdateCameraPosition()
    {
        if (cameraTransform != null)
        {
            Vector3 desiredPosition = transform.position + transform.TransformDirection(cameraOffset);
            cameraTransform.position = desiredPosition;
            cameraTransform.LookAt(transform.position + Vector3.up * cameraOffset.y);
        }
    }
}
