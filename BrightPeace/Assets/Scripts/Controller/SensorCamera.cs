using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorCamera : MonoBehaviour
{
    Camera camera;
    void Start()
    {
        transform.gameObject.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        camera = GetComponent<Camera>();
        camera.enabled = false;
    }
}
