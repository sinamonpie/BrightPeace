using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    public Material outlineMaterial;
    private Renderer[] playerRenderers;
    private List<Material[]> originalMaterials;

    void Start()
    {
        playerRenderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new List<Material[]>();

        foreach (Renderer renderer in playerRenderers)
        {
            originalMaterials.Add(renderer.materials);
        }
    }

    public void ApplyHighlight(float wallHackTime)
    {
        StartCoroutine(ShowHighlight(wallHackTime));
    }
    IEnumerator ShowHighlight(float wallHackTime)
    {
        SetHighlightMaterial(outlineMaterial);
        yield return new WaitForSeconds(wallHackTime);
        ResetHighlightMaterial();
    }

    void SetHighlightMaterial(Material mat)         // 각각 부위별 Renderer 설정
    {
        foreach (Renderer renderer in playerRenderers)
        {
            Material[] mats = new Material[renderer.materials.Length];
            for (int j = 0; j < renderer.materials.Length; j++)
            {
                mats[j] = mat;
            }
            renderer.materials = mats;
        }
    }

    void ResetHighlightMaterial()
    {
        for (int i = 0; i < playerRenderers.Length; i++)
        {
            playerRenderers[i].materials = originalMaterials[i];
        }
    }

}
