using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGameManager : MonoBehaviourPunCallbacks
{
    private static RoomGameManager instance = null;
    public bool isGame = false;

    public static RoomGameManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    IEnumerator CreatePlayer()
    {
        yield return new WaitUntil(() => isGame);

        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.one, Quaternion.identity, 0);
        if(PhotonNetwork.IsMasterClient)
        {
            player.GetComponent<PlayerState>().role = UserRole.Mental;
        }
        else
        {

        }
    }
}
