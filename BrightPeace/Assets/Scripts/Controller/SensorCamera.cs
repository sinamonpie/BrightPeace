using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorCamera : MonoBehaviour
{
    Camera camera;
    void Start()
    {
        camera = GetComponent<Camera>();
        camera.enabled = false;
    }

    public void SetCamera(bool enabled)
    {
        camera.enabled = enabled;
    }
}
