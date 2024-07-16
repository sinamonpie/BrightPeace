using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GrayScreen : MonoBehaviour
{
    public Volume volume;
    ColorAdjustments colorAdjustments;
    bool isEffectActive = false;

    void Start()
    {
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
    }

    public void ApplyGrayScreen(float applyTime)
    {
        if (!isEffectActive)
        {
            StartCoroutine(GrayscaleEffect(applyTime));
        }
    }

    IEnumerator GrayscaleEffect(float time)
    {
        Debug.Log("필터 적용");
        isEffectActive = true;
        colorAdjustments.saturation.value = -100f;
        yield return new WaitForSeconds(time);

        isEffectActive = false;
        colorAdjustments.saturation.value = 0f;
    }
}