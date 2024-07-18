using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody bulletRigid;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float destroyTime = 3f;
    void Start()
    {
        bulletRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        BulletMove();
    }

    void BulletMove()
    {
        bulletRigid.velocity = transform.forward * moveSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        destroyTime = 3f;
    }
}
