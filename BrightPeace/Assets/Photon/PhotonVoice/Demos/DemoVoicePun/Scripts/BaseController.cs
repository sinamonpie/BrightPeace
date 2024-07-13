// ----------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Exit Games GmbH">
// Photon Voice Demo for PUN- Copyright (C) 2016 Exit Games GmbH
// </copyright>
// <summary>
// Base class of character controllers.
// </summary>
// <author>developer@photonengine.com</author>
// ----------------------------------------------------------------------------

namespace ExitGames.Demos.DemoPunVoice
{
    using Photon.Pun;
    using UnityEngine;
    using UnityStandardAssets.CrossPlatformInput;

    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public abstract class BaseController : MonoBehaviour
    {
        public Camera ControllerCamera;

        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        protected Rigidbody rigidBody;
        protected Animator animator;
        protected Transform camTrans;             // A reference to transform of the third person camera

        private float _animationBlend;
        private int _animIDSpeed;
        private int _animIDMotionSpeed;

        private float h, v;

        [SerializeField]
        protected float speed = 5f;
        protected float SpeedChangeRate = 10.0f;

        [SerializeField]
        private float cameraDistance = 0f;

        protected virtual void OnEnable()
        {
            ChangePOV.CameraChanged += this.ChangePOV_CameraChanged;
        }

        protected virtual void OnDisable()
        {
            ChangePOV.CameraChanged -= this.ChangePOV_CameraChanged;
        }

        protected virtual void ChangePOV_CameraChanged(Camera camera)
        {
            if (camera != this.ControllerCamera)
            {
                this.enabled = false;
                this.HideCamera(this.ControllerCamera);
            }
            else
            {
                this.ShowCamera(this.ControllerCamera);
            }
        }

        protected virtual void Start()
        {
            PhotonView photonView = this.GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                this.Init();
                this.SetCamera();
            }
            else
            {
                this.enabled = false;
            }

        }
        //캐릭터 초기 컴포넌트 설정
        protected virtual void Init()
        {
            this.rigidBody = this.GetComponent<Rigidbody>();
            this.animator = this.GetComponent<Animator>();

            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }
        
        //카메라 설정
        protected virtual void SetCamera()
        {
            this.camTrans = this.ControllerCamera.transform;
            this.camTrans.position += this.cameraDistance * this.transform.forward;
        }
        //애니메이션 관련
        protected virtual void UpdateAnimator(float h, float v)
        {
            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = h != 0 || v != 0;
            // Tell the animator whether or not the player is walking.
            //this.animator.SetBool("IsWalking", walking);

            _animationBlend = Mathf.Lerp(_animationBlend, speed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            float dir = v >= 0 ? 1f : -1f;
            float inputMagnitude = walking ? 5f : 0f;

            animator.SetFloat(_animIDSpeed, dir * _animationBlend);
            animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }

        //발소리 관련 소스 *지우지 말 것*
        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.localPosition, FootstepAudioVolume);
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            // Store the input axes.
            this.h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            this.v = CrossPlatformInputManager.GetAxisRaw("Vertical");
#if MOBILE_INPUT
            if (Mathf.Abs(this.h) < 0.5f) { this.h = 0f; }
            else { this.h = Mathf.Sign(this.h); }
            if (Mathf.Abs(this.v) < 0.5f) { this.v = 0f; }
            else { this.v = Mathf.Sign(this.v); }
#endif  
            // send input to the animator
            this.UpdateAnimator(this.h, this.v);
            // Move the player around the scene.
            this.Move(this.h, this.v);
        }

        protected virtual void ShowCamera(Camera camera)
        {
            if (camera != null) { camera.gameObject.SetActive(true); }
        }

        protected virtual void HideCamera(Camera camera)
        {
            if (camera != null) { camera.gameObject.SetActive(false); }
        }
        // Move 가상함수
        protected abstract void Move(float h, float v);
    }
}
