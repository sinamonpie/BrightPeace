using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerState : MonoBehaviourPun
{
    [Header("실험체 체력")]
    [SerializeField] int maxHp = 2;
    [SerializeField] int currentHp;

    public UserRole role = UserRole.Patient;
    public bool isInCabinet;

    [SerializeField]
    private bool isDead;

    public Sound[] hallucinAudioClips;

    void Start()
    {
        InitHp();
    }

    void Update()
    {
    }

    public void SetMetal()
    {
        //if(photonView.IsMine)
        //    StartCoroutine(SetHearVoice());
    }

    IEnumerator SetHearVoice()
    {
        Debug.Log("SetHearVoice : " + System.DateTime.Now.ToString(("yyyy.MM.dd HH:mm:ss")));
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
        SetVoice(foundPlayer);
        StartCoroutine(SetHearVoice());
    }

    private void SetVoice(GameObject obj)
    {
        int soundIdx = Random.Range(0, hallucinAudioClips.Length);
        AudioSource.PlayClipAtPoint(hallucinAudioClips[soundIdx].clip, obj.transform.position, hallucinAudioClips[soundIdx].volume);

        return;

    }

    void InitHp()
    {
        isDead = false;
        if (photonView.Owner.IsMasterClient)
        {
            currentHp = 1;
        }
        else
        {
            currentHp = maxHp;
        }

        GameManager.Instance.SetRole(role);
    }

    public int GetPlayerHp()
    {
        return currentHp;
    }

    public void Heal(int heal)
    {
        currentHp += heal;
    }
    
    public void Dead()
    {
        isDead = true;
        if (role == UserRole.Patient)
        {
            InGameManager.Instance.DeadPatientPlayer();
        }
        else if (role == UserRole.Mental)
        {
            InGameManager.Instance.DeadMenetalPlayer();
        }

        InGameManager.Instance.GameEnding(UserEnding.DeadEnding);
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
            Dead();
        }
    }

    [PunRPC]
    void RPC_GetKill()
    {
        if (photonView.IsMine)
            GameManager.Instance.playKill++;
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
        GameManager.Instance.SetRole(role);
        SetMetal();
    }

    [PunRPC]
    void RPC_Stun(float duration)
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
        playerController.UnEnableMove();

        yield return new WaitForSeconds(duration);

        playerController.EnableMove();

    }
}
