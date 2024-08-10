using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
/// <summary>
/// 밸브를 가져와 끼우고 돌리면 지하실 문이 2분 후 열림
/// </summary>
public class ValveTank : MonoBehaviourPun
{
    [Header("지하실 문 열리는 시간(초)")]
    public float time = 120f;
    [SerializeField]
    private float currentTime = 0f;
    public float valveSpeed = 300f;

    [SerializeField] GameObject B1Door;
    [SerializeField] GameObject valve;
    [SerializeField] bool isvalve;
    public bool isUseValve;

    public GameObject usePlayer;

    bool checkCor;
    void Start()
    {
        // 밸브 
        valve = transform.GetChild(0).gameObject;
        valve.SetActive(false);
        isvalve = false;
        usePlayer = null;
    }

    public void SetValve()
    {
        photonView.RPC("RPC_SetValve", RpcTarget.All);
    }
    
    public bool GetValve()
    {
        return isvalve;
    }

    public void SetUsing(int _use)
    {
        photonView.RPC("RPC_SetUseValve", RpcTarget.All, _use);
    }

    public void NotUsing()
    {
        photonView.RPC("RPC_NotUseValve", RpcTarget.All);
    }

    public void OpenTheGate(bool isPartient)
    {
        if(isvalve)
        {
            photonView.RPC("RPC_OpenTheGate", RpcTarget.All, isPartient);
        }
    }

    IEnumerator SetValveDelay()
    {
        yield return new WaitForSeconds(0.5f);
        isvalve = true;
    }

    [PunRPC]
    void RPC_SetUseValve(int _ID)
    {
        usePlayer = PhotonView.Find(_ID).gameObject;
    }

    [PunRPC]
    void RPC_NotUseValve()
    {
        usePlayer = null;
    }

    [PunRPC]
    void RPC_SetValve()
    {
        valve.SetActive(true);
        StopCoroutine(SetValveDelay());
        StartCoroutine(SetValveDelay());
    }
    

    [PunRPC]
    void RPC_OpenTheGate(bool isPartient)
    {
        if (isPartient)
        {
            isUseValve = true;
        }
        else
        {
            isUseValve = false;
        }

        if (!checkCor)
        {
            StartCoroutine(OpenTheWaitGate(time));
        }
    }

    IEnumerator OpenTheWaitGate(float time)
    {
        // 0f
        checkCor = true;

        // 활성화 상태에서 2분 지나면 지하실 문 열림 
        while (currentTime < time)
        {
            if (isvalve)
            {
                if (isUseValve)
                {
                    currentTime += Time.deltaTime;
                    valve.transform.Rotate(Vector3.up, valveSpeed * Time.deltaTime);
                }
                else
                {
                    currentTime -= Time.deltaTime;
                    valve.transform.Rotate(Vector3.up, -valveSpeed * Time.deltaTime);

                    if (currentTime < 0f)
                    {
                        currentTime = 0f;
                        checkCor = false;
                        yield break;
                    }
                }
                GetComponentInChildren<DoorUseKeyUI>().DoorUI(time, currentTime);
                yield return null;
            }

        }

        // 지하실 문 열리기
        B1Door.GetComponent<EscapeEnding>().OpenEndingDoor();
        checkCor = false;
    }

}
