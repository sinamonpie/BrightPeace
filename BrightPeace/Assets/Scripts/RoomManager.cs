using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private static RoomManager instance = null;

    public Camera[] camears;
    public List<Transform> patientSpawn;
    public Transform securitySpawn;

    public GameObject[] gameObjects;

    public static RoomManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    private void Awake()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            camears[0].gameObject.SetActive(true);
            camears[1].gameObject.SetActive(false);
            GameObject obj = PhotonNetwork.Instantiate(gameObjects[0].name, securitySpawn.position, securitySpawn.rotation);
            obj.transform.SetParent(securitySpawn);
        }
        else
        {
            camears[0].gameObject.SetActive(false);
            camears[1].gameObject.SetActive(true);

            foreach(var trans in patientSpawn)
            {
                if(trans.childCount == 0)
                {
                    GameObject obj = PhotonNetwork.Instantiate(gameObjects[1].name, trans.position, trans.rotation);
                    obj.transform.SetParent(trans);
                    break;
                }
            }
        }
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
}
