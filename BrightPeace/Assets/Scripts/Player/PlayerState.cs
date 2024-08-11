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
    public bool isDead;

    [SerializeField]
    private bool isMentalSetting = false;

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
        if(!isMentalSetting)
        {
            StartCoroutine(SetHearVoice());
        }
    }

    IEnumerator SetHearVoice()
    {
        isMentalSetting = true;

        while(!isDead)
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
            SetVoice(foundPlayer);
        }
    }

    private void SetVoice(GameObject obj)
    {
        int soundIdx = Random.Range(0, hallucinAudioClips.Length);
        AudioSource.PlayClipAtPoint(hallucinAudioClips[soundIdx].clip, obj.transform.position, hallucinAudioClips[soundIdx].volume);
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
        this.transform.GetComponent<UseItemManager>().DieToDropItem();

        // 죽으면 Dead 엔딩
        InGameManager.Instance.DeadCountUp();
        InGameManager.Instance.GameEnding(role, UserEnding.DeadEnding);
    }

    public void Catch()
    {
        isDead = true;
        this.transform.GetComponent<UseItemManager>().DieToDropItem();

        // 잡히면 Lose 엔딩
        InGameManager.Instance.CatchCountUp();
        InGameManager.Instance.GameEnding(role, UserEnding.LoseEnding);
    }

    public void TakeDamage(int damage, UserRole _HitRole)
    {
        if (photonView.IsMine)
        {
            currentHp -= damage;
            if (currentHp <= 0)
            {
                if(_HitRole == UserRole.Security)
                {
                    Catch();
                }
                else
                {
                    Dead();
                }
            }
            photonView.RPC("RPC_TakeDamage", RpcTarget.Others, damage);
        }
    }

    [PunRPC]
    void RPC_TakeDamage(int damage)
    {
        currentHp -= damage;
        SoundManager.instance.PlayEffectAtPoint("PainSound", transform.position);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        if (photonView.IsMine)
            GameManager.Instance.playKill++;
    }

    public void SetRoleMental()
    {
        SetMetal();
        photonView.RPC("RPC_SetRoleMental", RpcTarget.All);
    }

    [PunRPC]
    void RPC_SetRoleMental()
    {
        role = UserRole.Mental;
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
        GameObject StunUIPrefab = InGameManager.Instance.StunUIPrefab;
        PlayerController playerController = GetComponent<PlayerController>();
        playerController.UnEnableMove();

        if (photonView.IsMine)
        {
            StunUIPrefab.SetActive(true);

        }

        yield return new WaitForSeconds(duration);

        playerController.EnableMove();

        if(photonView.IsMine)
        {
            StunUIPrefab.SetActive(false);
        }
    }
}
