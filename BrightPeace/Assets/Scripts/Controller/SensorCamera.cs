using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorCamera : MonoBehaviour
{
    void Start()
    {
        transform.gameObject.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
    }
}
