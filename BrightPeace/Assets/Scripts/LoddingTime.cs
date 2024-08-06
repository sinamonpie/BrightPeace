using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoddingTime : MonoBehaviour
{
    TMP_Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = GetCurrentDate();
    }
    public static string GetCurrentDate()
    {
        return DateTime.Now.ToString(("yyyy.MM.dd HH:mm:ss"));
    }

    public static string GetYear()
    {
        return DateTime.Now.ToString(("yyyy"));
    }
}
