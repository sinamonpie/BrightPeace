using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviourPunCallbacks
{
    public static InGameManager Instance { get; private set; }

    private bool isStart = false;
    public GameObject loadding;

    [Header("스폰할 열쇠 갯수")]
    public CountRange KeyCount;
    [Header("스폰할 총 갯수")]
    public CountRange GunCount;
    [Header("스폰할 아이템 갯수")]
    public CountRange ItemCount;


    [Header("스폰된 열쇠 갯수")]
    [SerializeField]
    private int spawnedKeyCount;
    [Header("스폰된 총 갯수")]
    [SerializeField]
    private int spawnedGunCount;
    [Header("스폰된 아이템 갯수")]
    [SerializeField]
    private int spawnedItemCount;


    [Space(20)]

    [Header("경비원 스폰 위치")]
    public Transform securitySpawn;
    [Header("환자 스폰 위치")]
    public Transform patientTransform;
    [Header("아이템 스폰 위치")]
    public Transform[] itemTransform;

    [Space(20)]

    [Header("경비원 프리팹")]
    public GameObject securityObject;
    [Header("환자 프리팹")]
    public GameObject patientObject;
    [Header("열쇠 프리팹")]
    public GameObject keyObject;
    [Header("총 프리팹")]
    public GameObject gunObject;
    [Header("나머지 아이템 프리팹")]
    public GameObject[] itemObjects;

    [Space(20)]

    [SerializeField]
    [Header("환자 스폰 위치들")]
    private Transform[] patientSpawn;

    [SerializeField]
    [Header("총 스폰 위치")]
    private Transform[] itemGunSpawn;
    [SerializeField]
    [Header("안 잠긴 방 스폰 위치")]
    private Transform[] itemUnLockSpawn;
    [SerializeField]
    [Header("잠긴 방 스폰 위치")]
    private Transform[] itemLockSpawn;

    public PhotonView pv;

    // Start is called before the first frame update
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        patientSpawn = GetChild(patientTransform);
        itemGunSpawn = GetChild(itemTransform[0]);
        itemUnLockSpawn = GetChild(itemTransform[1]);
        itemLockSpawn = GetChild(itemTransform[2]);

        SpawnPlayer();
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnItem();
        }
    }

    private void SpawnPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject player = PhotonNetwork.Instantiate(securityObject.name, securitySpawn.position, Quaternion.identity, 0);
        }
        else
        {
            int idx = UnityEngine.Random.Range(0, patientSpawn.Length);
            Vector3 spawnPosition = patientSpawn[idx].position;

            GameObject player = PhotonNetwork.Instantiate(patientObject.name, spawnPosition, Quaternion.identity, 0);
        }
    }
    
    private void SpawnItem()
    {
        spawnedKeyCount = UnityEngine.Random.Range(KeyCount.minCount, KeyCount.maxCount);
        spawnedGunCount = UnityEngine.Random.Range(GunCount.minCount, GunCount.maxCount);
        spawnedItemCount = UnityEngine.Random.Range(ItemCount.minCount, ItemCount.maxCount);

        //총 스폰
        for(int i = 0; i < spawnedGunCount; i++)
        {
            int idx = UnityEngine.Random.Range(0, itemGunSpawn.Length);
            Vector3 spawnPosition = itemGunSpawn[idx].position;

            PhotonNetwork.Instantiate(gunObject.name, spawnPosition, Quaternion.identity);

            itemGunSpawn = RemoveTransformAt(itemGunSpawn, idx);
            if (itemGunSpawn == null)
                break;
        }

        //키 스폰
        for (int i = 0; i < spawnedKeyCount; i++)
        {
            int idx = UnityEngine.Random.Range(0, itemUnLockSpawn.Length);
            Vector3 spawnPosition = itemUnLockSpawn[idx].position;

            PhotonNetwork.Instantiate(gunObject.name, spawnPosition, Quaternion.identity);

            itemUnLockSpawn = RemoveTransformAt(itemUnLockSpawn, idx);
            if (itemUnLockSpawn == null)
                break;
        }

        //아이템 스폰
        bool itemUnLock = false;
        bool itemLock = false;

        for (int i = 0; i < spawnedItemCount; i++)
        {
            int spawnIdx = UnityEngine.Random.Range(0, 2);
            if (itemUnLock)
                spawnIdx = 1;
            else if (itemLock)
                spawnIdx = 0;
            else if (itemUnLock && itemLock)
                break;

            if(spawnIdx == 0)
            {
                int idx = UnityEngine.Random.Range(0, itemUnLockSpawn.Length);
                Vector3 spawnPosition = itemUnLockSpawn[idx].position;
                PhotonNetwork.Instantiate(gunObject.name, spawnPosition, Quaternion.identity);

                itemUnLockSpawn = RemoveTransformAt(itemUnLockSpawn, idx);
                if (itemUnLockSpawn == null)
                    itemUnLock = true;
            }
            else
            {
                int idx = UnityEngine.Random.Range(0, itemLockSpawn.Length);
                Vector3 spawnPosition = itemLockSpawn[idx].position;
                PhotonNetwork.Instantiate(gunObject.name, spawnPosition, Quaternion.identity);

                itemLockSpawn = RemoveTransformAt(itemLockSpawn, idx);
                if (itemLockSpawn == null)
                    itemLock = true;
            }
        }
    }

    Transform[] GetChild(Transform _transform)
    {
        Transform[] _transforms = _transform.GetComponentsInChildren<Transform>();

        if(_transforms.Length <= 1)
        {
            return null;
        }

        List<Transform> _transformList = new List<Transform>(_transforms);
        _transformList.RemoveAt(0);

        _transforms = _transformList.ToArray();

        return _transforms;

    }

    Transform[] RemoveTransformAt(Transform[] _transforms, int idx)
    {
        if(_transforms.Length <= 1)
        {
            return null;
        }

        List<Transform> _transformList = new List<Transform>(_transforms);
        _transformList.RemoveAt(idx);

        _transforms = _transformList.ToArray();

        return _transforms;

    }


    // Update is called once per frame
    void Update()
    {
        if(!isStart)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                GameObject[] _players = GameObject.FindGameObjectsWithTag("Player");
                if(_players.Length == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    pv.RPC("GameStart", RpcTarget.All);
                }    
            }
        }
    }

    [PunRPC]
    void GameStart()
    {
        isStart = true;
        StartCoroutine(UnEnableLodding());
    }

    IEnumerator UnEnableLodding()
    {
        yield return new WaitForSeconds(3.0f);

        loadding.SetActive(false);
    }
}

[Serializable]
public class CountRange
{
    public int minCount;
    public int maxCount;
}
