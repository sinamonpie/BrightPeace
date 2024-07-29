using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadySlider : MonoBehaviour
{
    public Slider slider;

    public void SetMaxReadyCnt()
    {
        slider.maxValue = 4f;
        slider.value = 0f;
    }

    public void SetReadyCnt(float cnt)
    {
        slider.value = cnt;
    }

    public float GetReadyCnt()
    {
        return slider.value;
    }
}
