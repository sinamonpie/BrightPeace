// ----------------------------------------------------------------------------
// <copyright file="FirstPersonController.cs" company="Exit Games GmbH">
// Photon Voice Demo for PUN- Copyright (C) 2016 Exit Games GmbH
// </copyright>
// <summary>
// Custom fist person character controller.
// </summary>
// <author>developer@photonengine.com</author>
// ----------------------------------------------------------------------------

namespace ExitGames.Demos.DemoPunVoice
{

    using UnityEngine;
    using UnityEngine.EventSystems;

    public class FirstPersonController : BaseController
    {

        [SerializeField]
        private MouseLookHelper mouseLook = new MouseLookHelper();

        private float oldYRotation;
        private Quaternion velRotation;

        protected override void SetCamera()
        {
            base.SetCamera();
            this.mouseLook.Init(this.transform, this.camTrans);
        }

        //캐릭터 이동
        protected override void Move(float h, float v)
        {
            //카메라 방향 기준으로 이동
            this.dir = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(h * speed * Time.deltaTime, 0, v * speed * Time.deltaTime);

            if (!controller.isGrounded)
            {
                dir.y += _gravity * Time.deltaTime;
            }

            if (dir != Vector3.zero)
            {
                controller.Move(dir);
            }
        }

        private void Update()
        {
            this.RotateView();
        }
        
        private void RotateView()
        {
            // get the rotation before it's changed
            this.oldYRotation = this.transform.eulerAngles.y;
            this.mouseLook.LookRotation(this.transform, this.camTrans);
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            this.velRotation = Quaternion.AngleAxis(this.transform.eulerAngles.y - this.oldYRotation, Vector3.up);
        }
    }
}