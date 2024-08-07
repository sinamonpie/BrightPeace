using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : PlayerController
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

    //private Camera camera;
    public float cameraMaxDistance = 2f;
    float cameraDistance;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        avatarup = animator.GetBoneTransform(HumanBodyBones.Spine);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //camera = Camera.main;
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

        Camera.main.transform.localPosition = new Vector3(0, 0, -cameraDistance);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX * rotaionSpeed);

        verticalRotation += mouseY * rotaionSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 30f);

        cameraTransform.transform.localEulerAngles = Vector3.left * verticalRotation;

        AboidObstacle();
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
            currentSpeed = 4;
            realSpeed = SprintSpeed;
        }
        else if (z > 0)
        {
            currentSpeed = 2;
            realSpeed = moveSpeed;
        }
        else if (x > 0)
        {
            currentSpeed = 10;
            realSpeed = moveSpeed;
        }
        else if (x < 0)
        {
            currentSpeed = 8;
            realSpeed = moveSpeed;
        }
        else if (z < 0)
        {
            currentSpeed = -2;
            realSpeed = moveSpeed;
        }
        else
        {
            currentSpeed = 0;
        }

        animator.SetFloat("Speed", currentSpeed);
        animator.SetFloat("MotionSpeed", 1);

        characterController.Move(moveDirection * realSpeed * Time.deltaTime); ;
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

    
    void AboidObstacle()
    {
        RaycastHit hit;
        Vector3 dir = Camera.main.transform.position - cameraTransform.transform.position;
        Debug.DrawRay(cameraTransform.transform.position, dir.normalized * dir.magnitude, Color.red);
        if (Physics.Raycast(cameraTransform.transform.position, dir.normalized, out hit, dir.magnitude, LayerMask.GetMask("Default", "Object")))
        {
            Debug.Log(hit.transform.position);
            Vector3 dist = hit.point - cameraTransform.transform.position;
            cameraDistance = (dist.magnitude * 0.9f);
        }
        else
        {
            cameraDistance = Mathf.Clamp(cameraDistance + (0.8f * Time.deltaTime), -0.9f, cameraMaxDistance);
        }
    }
    
}
