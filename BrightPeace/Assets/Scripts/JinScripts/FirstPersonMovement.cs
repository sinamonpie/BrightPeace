using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : PlayerController
{
    [SerializeField]
    public float moveSpeed = 2.0f;
   
    [SerializeField]
    protected float SprintSpeed = 5.335f;

    Animator animator;

    public float rataionSpeed = 100;
    private Vector3 rotaion;

    private float currentSpeed;
    private Transform avatarup;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        avatarup = animator.GetBoneTransform(HumanBodyBones.Spine);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if(x > 0 || z > 0)
        {
            currentSpeed = moveSpeed;
        }
        else
        {
            currentSpeed = 0;
        }

        animator.SetFloat("Speed", currentSpeed);
        animator.SetFloat("MotionSpeed", 1);

        MoveTo(new Vector3(x, 0, z));

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
      
        rotaion = new Vector3(0, mouseX * rataionSpeed * Time.deltaTime, 0);
        transform.Rotate(rotaion);
    }

    public void MoveTo(Vector3 direction)
    {
        Vector3 movedis = transform.rotation * direction;
        moveDirection = new Vector3(movedis.x, moveDirection.y, movedis.z);

        if (characterController.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}