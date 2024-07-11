using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public InputField nickInput;
    public void JoinLobby()
    {
        string nick = nickInput.text;
        PhotonManager.Instance.JoinLobby(nick);
    }
}
