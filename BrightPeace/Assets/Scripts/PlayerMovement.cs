using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController controller;
    public float PlayerSpeed = 2f;

    private Vector3 velocity;
    public float Gravity = -9.81f;

    public Camera Camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Camera = Camera.main;
            Camera.GetComponent<FirstPersonCamera>().Target = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void FixedUpdateNetwork()
    {

        if(HasStateAuthority == false)
        {
            return;
        }

        //Is Grounded
        if (controller.isGrounded)
        {
            velocity = new Vector3(0, 0, 0);
        }

        Quaternion cameraRotaionY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);

        Vector3 move = cameraRotaionY * new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;

        velocity.y += Gravity * Runner.DeltaTime;
        
        controller.Move(move + velocity * Runner.DeltaTime);
        
        if(move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }
}
