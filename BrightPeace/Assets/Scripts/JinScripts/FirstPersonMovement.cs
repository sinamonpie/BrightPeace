using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : PlayerController
{
    private float realSpeed;

    Animator animator;

    public float rotaionSpeed = 3;
    private Vector3 rotaion;

    private float currentSpeed;

    private float verticalRotation = 0;
    
    private Transform avatarup;

    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    ActionController actionController;

    private bool isSwing = false;

    [SerializeField] private LayerMask ch_layerMask;

    [SerializeField]
    RaycastHit hit;
    Ray ray;

    [SerializeField]
    float swingRange = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        actionController = FindObjectOfType<ActionController>();
        avatarup = animator.GetBoneTransform(HumanBodyBones.Spine);

        if (!pv.IsMine)
            return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine)
            return;

        Cursor.visible = false;
        if (!isSwing)
        {
            Look();
            MoveTo();
            Attack();
        }
    }

    private void LateUpdate()
    {
        if (!pv.IsMine)
            return;
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

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            animator.SetTrigger("isSwing");
            IsSwing();

            ray = new Ray(transform.position + Vector3.up * 1f, transform.forward);

            if (Physics.Raycast(ray, out hit, swingRange))
            {
                if(hit.transform.CompareTag("Player"))
                {
                    PlayerState playerState = hit.transform.GetComponent<PlayerState>();

                    // 다른 플레이어가 맞았으면 
                    if (playerState != null)
                    {
                        pv.RPC("AttackPaintient", RpcTarget.All, hit.transform.GetComponent<PhotonView>().ViewID);
                    }
                }
            }
            Invoke("IsSwing", 2f);
        }
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
            currentSpeed = 8;
            realSpeed = moveSpeed;
        }
        else if (x < 0)
        {
            currentSpeed = 10;
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

    private void IsSwing()
    {
        isSwing = !isSwing;
    }

    [PunRPC]
    void AttackPaintient(int targetViewID)
    {
        PhotonView targetView = PhotonView.Find(targetViewID);
        if (targetView != null)
        {
            PlayerState playerHp = targetView.GetComponent<PlayerState>();
            if (playerHp != null)
            {
                playerHp.TakeDamage(1);
                Debug.Log("대상 남은 체력 : " + playerHp.GetPlayerHp().ToString());
            }
        }
    }
}