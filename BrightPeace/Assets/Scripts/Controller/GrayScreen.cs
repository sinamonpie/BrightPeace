using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class GrayScreen : MonoBehaviour
{
    PostProcessVolume postProcessVolume;
    ColorGrading colorGrading;
    bool isEffectActive = false;

    void Start()
    {
        postProcessVolume = FindObjectOfType<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);
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
        colorGrading.saturation.value = -100f;
        yield return new WaitForSeconds(time);

        isEffectActive = false;
        colorGrading.saturation.value = 0f;
    }
}