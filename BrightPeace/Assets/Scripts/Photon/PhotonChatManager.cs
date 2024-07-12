using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    private string userName;
    private string currentChannelName;

    private static PhotonChatManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public static PhotonChatManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    public void ChatConnect(string nick)
    {
        Application.runInBackground = true;

        userName = nick;

        chatClient = new ChatClient(this);
        chatClient.ChatRegion = "kr";
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
                        PhotonNetwork.AppVersion, new AuthenticationValues(userName));
    }


    /// <summary>
    /// ///////////////////////Photon Network Override/;//////////////////////////////////////////////////////////
    /// </summary>
    /// 
    #region IChatClientListener implementation
    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(level);
        Debug.Log(message);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("챗 상태 : " + state);
    }

    public void OnConnected()
    {
        Debug.Log("챗 연결완료 : " + userName);
        GameManager.Instance.LoadLobbyScene();
    }

    public void OnDisconnected()
    {
        Debug.Log("챗 연결 끊김 : " + userName);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            //if (InRoomManager.Instance != null)
            //{
            //    if (PhotonNetwork.MasterClient.NickName.Equals(senders[i]))
            //    {
            //        InRoomManager.Instance.setUserChat(senders[i], messages[i].ToString(), true);
            //    }
            //    else
            //    {
            //        InRoomManager.Instance.setUserChat(senders[i], messages[i].ToString(), false);
            //    }
            //}
            //if (RoomGameManager.Instance != null)
            //{
            //    if (PhotonNetwork.MasterClient.NickName.Equals(senders[i]))
            //    {
            //        RoomGameManager.Instance.setUserChat(senders[i], messages[i].ToString(), true);
            //    }
            //    else
            //    {
            //        RoomGameManager.Instance.setUserChat(senders[i], messages[i].ToString(), false);
            //    }
            //}
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string[] user = channelName.Split(":");

        //if (InRoomManager.Instance != null)
        //{
        //    bool isUse = false;

        //    if (!user[0].Equals(user[1]))
        //    {
        //        Dictionary<int, Photon.Realtime.Player> playerList = PhotonNetwork.CurrentRoom.Players;
        //        foreach (var player in playerList)
        //        {
        //            if (player.Value.NickName.Equals(user[1].Trim()))
        //            {
        //                isUse = true;
        //            }
        //        }

        //        if (isUse)
        //        {
        //            InRoomManager.Instance.setPrivateUserChat(sender, message.ToString());
        //        }
        //        else
        //        {
        //            InRoomManager.Instance.setNotice("해당하는 유저가 없습니다.", 0);
        //        }
        //    }
        //    else
        //    {
        //        InRoomManager.Instance.setNotice("자신에게 비밀챗을 보낼 수 없습니다.", 0);
        //    }
        //}

        if (RoomGameManager.Instance != null)
        {
            bool isUse = false;

            if (!user[0].Equals(user[1]))
            {
                Dictionary<int, Photon.Realtime.Player> playerList = PhotonNetwork.CurrentRoom.Players;
                foreach (var player in playerList)
                {
                    if (player.Value.NickName.Equals(user[1].Trim()))
                    {
                        isUse = true;
                    }
                }

                if (isUse)
                {
                   // RoomGameManager.Instance.setPrivateUserChat(sender, message.ToString());
                }
                else
                {
                  //  RoomGameManager.Instance.setNotice("해당하는 유저가 없습니다.");
                }
            }
            else
            {
              //  RoomGameManager.Instance.setNotice("자신에게 비밀챗을 보낼 수 없습니다.");
            }
        }
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log(string.Format("채널 입장 ({0})", string.Join(",", channels)));
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log(string.Format("채널 퇴장 ({0})", string.Join(",", channels)));
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
