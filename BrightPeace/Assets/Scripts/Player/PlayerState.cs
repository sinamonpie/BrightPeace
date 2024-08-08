using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerState : MonoBehaviourPun
{
    FirstPersonMovement firstPersonMovement;
    [Header("실험체 체력")]
    [SerializeField] int maxHp = 2;
    [SerializeField] int currentHp;
    public UserRole role = UserRole.Patient;
    public bool isInCabinet;

    public AudioClip hallucinAudioClips;
    [Range(0, 1)] public float hallucinStepAudioVolume = 0.8f;

    void Start()
    {
        firstPersonMovement = GetComponent<FirstPersonMovement>();
        InitHp();
    }

    void Update()
    {
    }

    public void SetMetal()
    {
        StartCoroutine(SetHearVoice());
    }

    IEnumerator SetHearVoice()
    {
        float time = Random.Range(60f, 120f);
        yield return new WaitForSeconds(time);

        GameObject[] _players = GameObject.FindGameObjectsWithTag("Player");
        float shortDis = Vector3.Distance(transform.position, _players[0].transform.position);

        GameObject foundPlayer = _players[0];
        foreach (GameObject found in _players)
        {
            if (found != this)
            {
                float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);
                if (Distance < shortDis)
                {
                    shortDis = Distance;
                    foundPlayer = found;
                }
            }
        }

    }

    void SetVoice(GameObject obj)
    {
        AudioSource.PlayClipAtPoint(hallucinAudioClips, transform.TransformPoint(obj.transform.position), hallucinStepAudioVolume);
    }

    void InitHp()
    {
        if (photonView.Owner.IsMasterClient)
        {
            currentHp = 1;
        }
        else
        {
            currentHp = maxHp;
        }
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

    public void SetRoleMental()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_SetRoleMental", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_SetRoleMental()
    {
        role = UserRole.Mental;
    }

    [PunRPC]
    public void RPC_Stun(float duration)
    {
        StartCoroutine(ClientPlayerStun(duration));
    }

    public bool IsInCabinet()
    {
        return isInCabinet;
    }

    public void PlayerInCabinet()
    {
        isInCabinet = !isInCabinet;
    }

    IEnumerator ClientPlayerStun(float duration)
    {
        PlayerController playerController = GetComponent<PlayerController>();
        float moveSpeed = playerController.moveSpeed;
        float sprintSpeed = playerController.SprintSpeed;

        playerController.moveSpeed = 0f;
        playerController.SprintSpeed = 0f;

        yield return new WaitForSeconds(duration);

        playerController.moveSpeed = moveSpeed;
        playerController.SprintSpeed = sprintSpeed;

    }
}
