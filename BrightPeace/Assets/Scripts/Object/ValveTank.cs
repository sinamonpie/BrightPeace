using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 밸브를 가져와 끼우고 돌리면 지하실 문이 2분 후 열림
/// </summary>
public class ValveTank : MonoBehaviour
{
    [Header("지하실 문 열리는 시간")]
    public float time = 120f;
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
        valve.SetActive(true);
        StopCoroutine(SetValveDelay());
        StartCoroutine(SetValveDelay());
    }
    
    public bool GetValve()
    {
        return isvalve;
    }

    public void OpenTheGate()
    {
        if(isvalve)
        {
            StartCoroutine(OpenTheWaitGate(time));
        }
    }

    IEnumerator SetValveDelay()
    {
        yield return new WaitForSeconds(0.5f);
        isvalve = true;
    }

    IEnumerator OpenTheWaitGate(float time)
    {
        Debug.Log("잘 됨 수구");
        yield return new WaitForSeconds(time);
        // 지하실 문 열리기
    }
}
