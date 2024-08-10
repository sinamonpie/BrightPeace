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

    public void DoorUI(float totalTime, float currnetTime, bool isPartient)
    {
        image.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        StartCoroutine(UseValveUI(totalTime, currnetTime, isPartient));
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

    IEnumerator UseValveUI(float totalTime, float currentTime, bool isPartient)
    {
        StartCoroutine(TankUseText(totalTime, currentTime, isPartient));

        while (currentTime < totalTime && currentTime >= 0)
        {
            if (isPartient)
            {
                image.fillAmount = currentTime / totalTime;
                currentTime += Time.deltaTime;
            }
            else
            {
                image.fillAmount = 1f - (currentTime / totalTime);
                currentTime -= Time.deltaTime;

                if (currentTime < 0f)
                {
                    currentTime = 0f;
                    image.fillAmount = 1f; 
                    yield break;
                }
            }

            yield return null;
        }

        if (isPartient)
        {
            image.fillAmount = 0f; 
        }
        else
        {
            image.fillAmount = 1f; 
        }
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

    IEnumerator TankUseText(float totalTime, float currentTime, bool isPartient)
    {

        while (currentTime >= 0 && currentTime <= totalTime)
        {
            // 진행도를 계산하여 %로 변환
            float progress;

            if (isPartient)
            {
                progress = currentTime / totalTime;
            }
            else
            {
                progress = 1f - (currentTime / totalTime);
            }

            int percentage = Mathf.FloorToInt(progress * 100); // 0% ~ 100%로 변환

            // 텍스트를 "n%" 형식으로 표시
            text.text = string.Format("진행도\n{0}%", percentage);

            yield return new WaitForSeconds(1f);

            // 현재 시간을 갱신
            currentTime = isPartient ? currentTime + 1f : currentTime - 1f;

        }

        text.text = isPartient ? "진행도\n100%" : "진행도\n0%";
    }

    IEnumerator TankUseText(float time)
    {
        float totalTime = time;

        while (time > 0)
        {
            // 진행도를 계산하여 %로 변환
            float progress = 1f - (time / totalTime);
            int percentage = Mathf.FloorToInt(progress * 100);

            text.text += string.Format("{0}%", percentage);

            yield return new WaitForSeconds(1f);

            time -= 1f;
        }


        text.text = "100%";
    }
}
