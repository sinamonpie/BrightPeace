using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float rotateSpeedX = 3f;
    private float rotateSpeedY = 5f;
    private float limitMinX = -80f;
    private float limitMaxX = 50f;
    private float eulerAngleX;
    private float eulerAngleY;

    public Transform Target;

    public void RotateTo(float mouseX,  float mouseY)
    {
        eulerAngleY = mouseX * rotateSpeedX;
        eulerAngleX -= mouseY * rotateSpeedY;

        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        Target.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

}
