using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
    void Start()
    {
        // 밸브 
        valve = transform.GetChild(0).gameObject;
        valve.SetActive(false);
        isvalve = false;
    }

    public void SetValve()
    {
        photonView.RPC("RPC_SetValve", RpcTarget.All);
    }
    
    public bool GetValve()
    {
        return isvalve;
    }

    public void OpenTheGate()
    {
        if(isvalve)
        {
            photonView.RPC("RPC_OpenTheGate", RpcTarget.All);
        }
    }

    IEnumerator SetValveDelay()
    {
        yield return new WaitForSeconds(0.5f);
        isvalve = true;
    }

    [PunRPC]
    void RPC_SetValve()
    {
        valve.SetActive(true);
        StopCoroutine(SetValveDelay());
        StartCoroutine(SetValveDelay());
    }
    

    [PunRPC]
    void RPC_OpenTheGate()
    {
        StartCoroutine(OpenTheWaitGate(time));
    }
    IEnumerator OpenTheWaitGate(float time)
    {
        // 밸브 돌아감
        StartCoroutine(RotateValve(time));
        GetComponentInChildren<DoorUseKeyUI>().DoorUI(time);
        yield return new WaitForSeconds(time);
        // 지하실 문 열리기
    }

    IEnumerator RotateValve(float duration)
    {
        currentTime = 0f;
        while (currentTime < duration)
        {
            valve.transform.Rotate(Vector3.up, valveSpeed * Time.deltaTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        yield break;
    }
}
