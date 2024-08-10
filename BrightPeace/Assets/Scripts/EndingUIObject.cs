using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingUIObject : MonoBehaviour
{
    public TMP_Text playtext; 
    public TMP_Text datetext;

    public bool isTime = false;
    private short KillCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        playtext = GetComponent<TMP_Text>();
        datetext = GetComponent<TMP_Text>();
    }
    private void OnEnable()
    {
        playtext.text = PhotonNetwork.LocalPlayer.NickName;
        if (isTime)
        {
            datetext.text = DateTime.Now.ToString("yyyy년 MM월 dd일");
        }
        else
        {
            datetext.text = DateTime.Now.ToString("yyyy년 MM월 dd일 HH시 mm분");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
