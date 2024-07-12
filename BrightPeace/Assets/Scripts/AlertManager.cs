using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    public TMP_Text message;

    public void SetMessage(string _message)
    {
        message.text = _message;
        gameObject.SetActive(true);
    }

    public void OKBtn()
    {
        gameObject.SetActive(false);
    }
}
