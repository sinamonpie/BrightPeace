using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
/// <summary>
/// 잠긴 문에 n초간 열리는 UI 생성
/// </summary>
public class DoorUseKeyUI : MonoBehaviourPun
{
    [SerializeField] Image image;     
    [SerializeField] TMP_Text text;
    public bool isDoor;
    void Start()
    {
        image = transform.GetComponentInChildren<Image>();
        text = transform.GetComponentInChildren<TMP_Text>();

        image.gameObject.SetActive(false);
        text.gameObject.SetActive(false);

        if (isDoor)
        {
            this.gameObject.transform.position = transform.parent.position;
            this.gameObject.transform.position += new Vector3(-0.5f, 0f, 0.3f);
            this.gameObject.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180f, transform.rotation.eulerAngles.z);
        }
        else
        {
            text.text = "";
        }

    }

    public void DoorUI(float time)
    {
        image.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        StartCoroutine(DoorUseKey(time));
    }

    IEnumerator DoorUseKey(float time)
    {            
        if(isDoor)
        {
            StartCoroutine(DoorUseKeyText(time));
        }
        else
        {
            StartCoroutine(TankUseText(time));
        }

        float duration = time;

        while(time > 0.0f)
        {
            time -= Time.deltaTime;
            image.fillAmount = (time / duration);
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

    IEnumerator TankUseText(float time)
    {
        while (time > 0)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            text.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            yield return new WaitForSeconds(1f);

            time -= 1f;
        }

        // 카운트다운이 끝난 후 "0:00"으로 표시
        text.text = "0:00";
    }
}
