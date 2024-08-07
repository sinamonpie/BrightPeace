using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : PlayerController
{
    [SerializeField]
    public float moveSpeed = 2.0f;
   
    [SerializeField]
    protected float SprintSpeed = 5.335f;

    private float realSpeed;

    Animator animator;

    public float rotaionSpeed = 3;
    private Vector3 rotaion;

    private float currentSpeed;

    private float verticalRotation = 0;
    
    private Transform avatarup;

    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        avatarup = animator.GetBoneTransform(HumanBodyBones.Spine);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;

        Look();
        MoveTo();
    }

    private void LateUpdate()
    {
        avatarup.localRotation = Quaternion.Euler(-verticalRotation, 0, 0);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX * rotaionSpeed);

        verticalRotation += mouseY * rotaionSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, -25f, 30f);

        cameraTransform.transform.localEulerAngles = Vector3.left * verticalRotation;
    }

    public void MoveTo()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(x, 0, z);

        Vector3 movedis = transform.rotation * direction;
        moveDirection = new Vector3(movedis.x, moveDirection.y, movedis.z);

        if (characterController.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift) && z > 0)
        {
            currentSpeed = 6;
            realSpeed = SprintSpeed;
        }
        else if (z > 0)
        {
            currentSpeed = 2;
            realSpeed = moveSpeed;
        }
        else if (x > 0)
        {
            currentSpeed = 14;
            realSpeed = moveSpeed;
        }
        else if (x < 0)
        {
            currentSpeed = 10;
            realSpeed = moveSpeed;
        }
        else if (z < 0)
        {
            currentSpeed = 18;
            realSpeed = moveSpeed;
        }
        else
        {
            currentSpeed = 0;
        }

        animator.SetFloat("Speed", currentSpeed);
        animator.SetFloat("MotionSpeed", 1);
        
        characterController.Move(moveDirection * realSpeed * Time.deltaTime);

    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(characterController.center), FootstepAudioVolume);
            }
        }
    }
}