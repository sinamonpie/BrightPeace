using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform Target;

    public float MouseSensitivity = 12f;

    private float verticalRotaion;
    private float horizontalRotaion;

    private void LateUpdate()
    {
        if (Target == null)
            return;

        transform.position = Target.position;

        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");

        verticalRotaion -= MouseY * MouseSensitivity;
        verticalRotaion = Mathf.Clamp(verticalRotaion, -70f, 7);

        horizontalRotaion += MouseX * MouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotaion, horizontalRotaion, 0f);
    }

    
}
