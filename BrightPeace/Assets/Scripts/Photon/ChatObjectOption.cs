using TMPro;
using UnityEngine;

public class ChatObjectOption : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text nickNameObject;
    public TMP_Text messageObject;
    public GameObject masterObject;

    public void SetMessage(string nickName, string message, bool isMaster)
    {
        nickNameObject.text = nickName;
        messageObject.text = message;
        if (isMaster)
        {
            masterObject.SetActive(true);
        }
    }

    public void SetMessage(string nickName, string message)
    {
        nickNameObject.text = nickName;
        messageObject.text = message;
    }

    public void SetNotice(string message)
    {
        messageObject.text = message;
    }
}