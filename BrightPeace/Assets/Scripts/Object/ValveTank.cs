using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 밸브를 가져와 끼우고 돌리면 지하실 문이 2분 후 열림
/// </summary>
public class ValveTank : MonoBehaviour
{
    [Header("지하실 문 열리는 시간(초)")]
    public float time = 120f;
    [Header("밸브 돌아가는 속도")]
    public float valveSpeed = 100f;

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
        // 밸브 돌아감
        RotateValve(time);
        yield return new WaitForSeconds(time);
        // 지하실 문 열리기
    }

    IEnumerator RotateValve(float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            transform.Rotate(Vector3.up, valveSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
