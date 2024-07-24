using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    int maxHp = 2;
    public int currentHp = 0;
    void Start()
    {
        InitHp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitHp()
    {
        currentHp = maxHp;
    }
}
