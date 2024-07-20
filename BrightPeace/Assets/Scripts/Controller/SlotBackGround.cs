using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotBackGround : MonoBehaviour
{
    private Image image;
    private Color color;
    void Start()
    {
        image = transform.GetComponent<Image>();
        color = image.color;
    }

    public void SetSlot()
    {
        SetColor(255f, 0f, 0f, 0.85f);
    }

    public void DisSlot()
    {
        image.color = color;
    }

    void SetColor(float r, float g, float b, float a)
    {
        Color color = new Color(r, g, b, a);
        image.color = color;
    }
}
