using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerState : MonoBehaviourPun
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
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        }
    }

    [PunRPC]
    void RPC_TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            this.gameObject.SetActive(false);
        }
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
