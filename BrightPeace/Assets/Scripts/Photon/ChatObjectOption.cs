using TMPro;
using UnityEngine;

public class ChatObjectOption : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text nickNameObject;
    public TMP_Text privateMine;
    public TMP_Text messageObject;

    public void SetMessage(string nickName, string message, bool isMaster)
    {
        if(isMaster)
        {
            nickNameObject.color = Color.red;
            messageObject.color = Color.red;
        }
        nickNameObject.text = nickName;
        messageObject.text = message;
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

    public void SetPrivateMessage(string nickName, string message, bool isMine)
    {
        nickNameObject.text = nickName;
        messageObject.text = message;
        if(isMine)
        {
            privateMine.text = ">>";
        }
        else
        {
            privateMine.text = "<<";
        }
    }
}