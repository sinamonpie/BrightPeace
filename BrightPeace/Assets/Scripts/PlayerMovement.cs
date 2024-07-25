using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController controller;
    public float PlayerSpeed = 2f;

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

        Quaternion cameraRotaionY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);

        Vector3 move = cameraRotaionY * new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;
   
        controller.Move(move);
        
        if(move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }
}
