using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMove : MonoBehaviour
{
    public float speed = 1f;
    public float distance = 5f; 

    private Vector3 startPosition;
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newPositionX = startPosition.x + Mathf.PingPong(Time.time * speed, distance) * direction;
        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);

        if (Mathf.Abs(transform.position.x - startPosition.x) >= distance)
        {
            direction *= -1;
        }
    }
}
