using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
/// <summary>
/// 잠긴 문에 2초간 열리는 UI 생성
/// </summary>
public class DoorUseKeyUI : MonoBehaviourPun
{
    [SerializeField] Image image;     
    [SerializeField] TMP_Text text;      
    void Start()
    {
        this.gameObject.transform.position = transform.parent.position;
        image = transform.GetComponentInChildren<Image>();
        text = transform.GetComponentInChildren<TMP_Text>();

        image.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void DoorUI(float time)
    {
        photonView.RPC("RPC_ShowUI", RpcTarget.All, time);
    }
    IEnumerator DoorUseKey(float time)
    {            
        StartCoroutine(DoorUseKeyText(time));
        while(time > 0.0f)
        {
            time -= Time.deltaTime;
            image.fillAmount = (time / 1.0f);
            yield return new WaitForFixedUpdate();
        }
        image.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    IEnumerator DoorUseKeyText(float time)
    {
        float textTime = time / 3.0f;
        while(time > 0.0f)
        {
            time -= Time.deltaTime;
            text.text += ".";
            yield return new WaitForSeconds(textTime);
        }
    }

    [PunRPC]
    void RPC_ShowUI(float time)
    {
        image.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        StartCoroutine(DoorUseKey(time));
    }
}
