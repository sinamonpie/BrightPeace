using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] int maxHp = 2;
    [SerializeField] int currentHp;
    public UserRole role = UserRole.Patient;
    public bool isInCabinet;

    void Start()
    {
        InitHp();
    }

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

    public bool IsInCabinet()
    {
        return isInCabinet;
    }

    public void PlayerInCabinet()
    {
        isInCabinet = !isInCabinet;
    }
}
