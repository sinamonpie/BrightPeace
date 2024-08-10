using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using static UnityEngine.Rendering.DebugUI;
/// <summary>
/// 잠긴 문에 n초간 열리는 UI 생성
/// </summary>
public class DoorUseKeyUI : MonoBehaviourPun
{
    [SerializeField] Image image;     
    [SerializeField] TMP_Text text;
    public bool isDoor;
    bool isPartient;
    float currentTime = 0f;
    bool checkCor;
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

    public void DoorUI(float totalTime, float currentTime)
    {
        image.gameObject.SetActive(true);
        text.gameObject.SetActive(true);

        image.fillAmount = currentTime / totalTime;

        float progress = currentTime / totalTime;
        int percentage = Mathf.FloorToInt(progress * 100); // 0% ~ 100%로 변환
        text.text = string.Format("진행도\n{0}%", percentage);
    }

    IEnumerator DoorUseKey(float time)
    {            
        if(isDoor)
        {
            StartCoroutine(DoorUseKeyText(time));
        }
        else
        {
            /*StartCoroutine(TankUseText(time));*/
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

/*    IEnumerator UseValveUI(float totalTime)
    {
        float progress;

        checkCor = true;
        image.gameObject.SetActive(true);
        text.gameObject.SetActive(true);

        while (currentTime < totalTime && currentTime >= 0)
        {
            if (this.isPartient)
            {
                progress = currentTime / totalTime;
                image.fillAmount = currentTime / totalTime;
                currentTime += Time.deltaTime;
            }
            else
            {
                progress = 1f - (currentTime / totalTime);
                image.fillAmount = 1f - (currentTime / totalTime);
                currentTime -= Time.deltaTime;

                if (currentTime < 0f)
                {
                    currentTime = 0f;
                    checkCor = false;
                    image.fillAmount = 1f;
                    text.text = "진행도\n0%";
                    yield break;
                }
            }
            int percentage = Mathf.FloorToInt(progress * 100); // 0% ~ 100%로 변환
            text.text = string.Format("진행도\n{0}%", percentage);

            yield return null;
        }

        checkCor = false;
    }*/


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
}
