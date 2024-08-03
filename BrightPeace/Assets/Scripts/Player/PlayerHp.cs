using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    [SerializeField]
    int maxHp = 2;
    [SerializeField]
    int currentHp;
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

    public int GetPlayerHp()
    {
        return currentHp;
    }

    public void Heal(int heal)
    {
        currentHp += heal;
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
    }


}
