using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class PrefabsItem
{
    public GameObject obj;
    public int minCount;
    public int maxCount;
}

public class InGameManager : MonoBehaviourPunCallbacks
{
    public static InGameManager Instance { get; private set; }

    public GameObject loadding;

    [Header("스폰할 열쇠 갯수")]
    public PrefabsItem KeyItem;
    [Header("스폰할 퓨즈박스 갯수")]
    public PrefabsItem FuzeBoxItem;
    [Header("스폰할 총 갯수")]
    public PrefabsItem GunItem;
    [Header("스폰할 아이템 갯수")]
    public PrefabsItem[] Items;


    [Header("스폰된 열쇠 갯수")]
    [SerializeField]
    private int spawnedKeyCount;
    [Header("스폰된 퓨즈박스 갯수")]
    [SerializeField]
    private int spawnedFuzeBoxCount;
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

    [Space(20)]

    [SerializeField]
    [Header("환자 스폰 위치들")]
    private Transform[] patientSpawn;

    [SerializeField]
    [Header("퓨즈박스 스폰 위치")]
    private Transform[] itemFuzeBoxSpawn;
    [SerializeField]
    [Header("안 잠긴 방 스폰 위치")]
    private Transform[] itemUnLockSpawn;
    [SerializeField]
    [Header("잠긴 방 스폰 위치")]
    private Transform[] itemLockSpawn;

    public GameObject StunUIPrefab;
    public GameObject doorLockUI;
    public GameObject SlotParents;
    public GameObject CrossHairImage;
    public TMP_Text alertText;
    public PhotonView pv;

    private Player masterClient;

    [SerializeField]
    private GameObject[] _players;

    [SerializeField]
    private int patientCount;

    [SerializeField]
    private bool isDeadMental = true;

    [SerializeField]
    private bool isSetting;

    [SerializeField]
    private int deadCount = 0;

    [SerializeField]
    private int catchCount = 0;

    [SerializeField]
    private int aliveCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (AudioSource _obj in FindObjectsOfType<AudioSource>())
        {
            _obj.volume = 0;
        }

        if (patientSpawn.Length == 0)
            patientSpawn = GetChild(patientTransform);

        if (PhotonNetwork.IsMasterClient)
        {
            itemFuzeBoxSpawn = GetChild(itemTransform[0]);
            itemUnLockSpawn = GetChild(itemTransform[1]);
            itemLockSpawn = GetChild(itemTransform[2]);
        }

        pv = GetComponent<PhotonView>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        masterClient = PhotonNetwork.MasterClient;
    }

    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject player = PhotonNetwork.Instantiate(securityObject.name, securitySpawn.position, Quaternion.identity, 0);

            Transform caemraTrans = player.GetComponent<PlayerController>().GetCameraTransform();

            Camera.main.transform.position = caemraTrans.position;
            Camera.main.transform.rotation = caemraTrans.rotation;
            Camera.main.transform.SetParent(caemraTrans);
            Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("Security"));
            Camera.main.transform.GetComponent<ActionController>().SetPlayer();
        }
        else
        {
            int idx = UnityEngine.Random.Range(0, patientSpawn.Length);
            Vector3 spawnPosition = patientSpawn[idx].position;

            GameObject player = PhotonNetwork.Instantiate(patientObject.name, spawnPosition, Quaternion.identity, 0);

            if (PhotonNetwork.CurrentRoom.PlayerCount >= 3 && isDeadMental)
            {
                player.GetComponent<PlayerState>().SetRoleMental();
                pv.RPC("SetMentalSpawn", RpcTarget.AllBuffered);
            }
            else
            {
                int m = UnityEngine.Random.Range(0, 2);
                if (m == 0)
                {
                    player.GetComponent<PlayerState>().SetRoleMental();
                    pv.RPC("SetMentalSpawn", RpcTarget.AllBuffered);
                }
            }

            Transform caemraTrans = player.GetComponent<PlayerController>().GetCameraTransform();

            Camera.main.transform.position = caemraTrans.position;
            Camera.main.transform.rotation = caemraTrans.rotation;
            Camera.main.transform.SetParent(caemraTrans);
            Camera.main.transform.GetComponent<ActionController>().SetPlayer();

            Camera.main.transform.localPosition = new Vector3(0, 0, -3);
            // ㄴ 2024.08.07 14:01 테스트로 넣어본 문장

            //pv.RPC("RemoveSpawnPlayerList", RpcTarget.AllBuffered, idx);
        }
    }

    private void SpawnGun()
    {
        if (itemUnLockSpawn != null)
        {
            int idx = UnityEngine.Random.Range(0, itemUnLockSpawn.Length);
            Vector3 spawnPosition = itemUnLockSpawn[idx].position;

            PhotonNetwork.Instantiate(GunItem.obj.name, spawnPosition, Quaternion.identity);

            itemUnLockSpawn = RemoveTransformAt(itemUnLockSpawn, idx);
        }
        else if (itemLockSpawn != null)
        {
            int idx = UnityEngine.Random.Range(0, itemLockSpawn.Length);
            Vector3 spawnPosition = itemLockSpawn[idx].position;
            PhotonNetwork.Instantiate(GunItem.obj.name, spawnPosition, Quaternion.identity);

            itemLockSpawn = RemoveTransformAt(itemLockSpawn, idx);
        }
    }

    private void SpawnItem()
    {
        spawnedKeyCount = UnityEngine.Random.Range(KeyItem.minCount, KeyItem.maxCount);
        spawnedFuzeBoxCount = UnityEngine.Random.Range(FuzeBoxItem.minCount, FuzeBoxItem.maxCount);

        //퓨즈박스 스폰
        for (int i = 0; i < spawnedFuzeBoxCount; i++)
        {
            int idx = UnityEngine.Random.Range(0, itemFuzeBoxSpawn.Length);
            Vector3 spawnPosition = itemFuzeBoxSpawn[idx].position;
            Quaternion spawnRotation = itemFuzeBoxSpawn[idx].rotation;

            PhotonNetwork.Instantiate(FuzeBoxItem.obj.name, spawnPosition, spawnRotation);

            itemFuzeBoxSpawn = RemoveTransformAt(itemFuzeBoxSpawn, idx);
            if (itemFuzeBoxSpawn == null)
                break;
        }

        //키 스폰
        for (int i = 0; i < spawnedKeyCount; i++)
        {
            int idx = UnityEngine.Random.Range(0, itemUnLockSpawn.Length);
            Vector3 spawnPosition = itemUnLockSpawn[idx].position;

            PhotonNetwork.Instantiate(KeyItem.obj.name, spawnPosition, Quaternion.identity);

            itemUnLockSpawn = RemoveTransformAt(itemUnLockSpawn, idx);
            if (itemUnLockSpawn == null)
                break;
        }

        //아이템 스폰
        bool itemUnLock = false;
        bool itemLock = false;

        spawnedItemCount = 0;
        for (int i = 0; i < Items.Length; i++)
        {
            int spwanItemCnt = UnityEngine.Random.Range(Items[i].minCount, Items[i].maxCount);
            PrefabsItem itemObject = Items[i];
            for (int j = 0; j < spwanItemCnt; j++)
            {
                int spawnIdx = UnityEngine.Random.Range(0, 2);
                if (itemUnLock)
                    spawnIdx = 1;
                else if (itemLock)
                    spawnIdx = 0;
                else if (itemUnLock && itemLock)
                    break;

                if (spawnIdx == 0)
                {
                    int idx = UnityEngine.Random.Range(0, itemUnLockSpawn.Length);
                    Vector3 spawnPosition = itemUnLockSpawn[idx].position;
                    PhotonNetwork.Instantiate(itemObject.obj.name, spawnPosition, Quaternion.identity);

                    itemUnLockSpawn = RemoveTransformAt(itemUnLockSpawn, idx);
                    if (itemUnLockSpawn == null)
                        itemUnLock = true;
                }
                else
                {
                    int idx = UnityEngine.Random.Range(0, itemLockSpawn.Length);
                    Vector3 spawnPosition = itemLockSpawn[idx].position;
                    PhotonNetwork.Instantiate(itemObject.obj.name, spawnPosition, Quaternion.identity);

                    itemLockSpawn = RemoveTransformAt(itemLockSpawn, idx);
                    if (itemLockSpawn == null)
                        itemLock = true;
                }
            }
            spawnedItemCount += spwanItemCnt;
        }
    }

    Transform[] GetChild(Transform _transform)
    {
        Transform[] _transforms = _transform.GetComponentsInChildren<Transform>();

        if (_transforms.Length <= 1)
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
        if (_transforms.Length <= 1)
        {
            return null;
        }

        List<Transform> _transformList = new List<Transform>(_transforms);
        _transformList.RemoveAt(idx);

        _transforms = _transformList.ToArray();

        return _transforms;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.Instance.isGameStart)
        {
            if (PhotonNetwork.IsMasterClient && !isSetting)
            {
                Setting();
            }
        }
    }

    public void Setting()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        if (_players.Length == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            isSetting = true;

            SpawnItem();

            pv.RPC("GameStart", RpcTarget.All, PhotonNetwork.CurrentRoom.PlayerCount - 1);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (otherPlayer == masterClient)
            {
                PhotonManager.Instance.MasterClientDisconnect();
                return;
            }
        }

        Debug.Log(otherPlayer.NickName);
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        SecurityEnding();
    }

    public void GameEnding(UserRole _role, UserEnding _end)
    {
        GameManager.Instance.SetEnding(_role, _end);
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("MentalWin", RpcTarget.Others);
        }
        PhotonNetwork.LeaveRoom();
    }

    public void CatchCountUp(UserRole _role)
    {
        if (_role == UserRole.Mental)
            isDeadMental = true;

        pv.RPC("RPC_CatchCountUp", RpcTarget.All, catchCount + 1, isDeadMental);
    }

    public void DeadCountUp(UserRole _role)
    {
        if (_role == UserRole.Mental)
            isDeadMental = true;

        pv.RPC("RPC_DeadCountUp", RpcTarget.All, deadCount + 1, isDeadMental);
    }

    public void AliveCountUp(UserRole _role)
    {
        if (_role == UserRole.Mental)
            isDeadMental = true;

        pv.RPC("RPC_AliveCountUp", RpcTarget.All, aliveCount + 1, isDeadMental);
    }

    public int GetAliveCount()
    {
        return aliveCount;
    }

    public int GetDeadCount()
    {
        return deadCount;
    }

    public int GetCatchCount()
    {
        return catchCount;
    }

    public int GetAllCount()
    {
        return aliveCount + deadCount + catchCount;
    }

    [PunRPC]
    void RPC_DeadCountUp(int _cnt, bool _isDeadMantal)
    {
        isDeadMental = _isDeadMantal;
        deadCount = _cnt;
    }

    [PunRPC]
    void RPC_CatchCountUp(int _cnt, bool _isDeadMantal)
    {
        isDeadMental = _isDeadMantal;
        catchCount = _cnt;
    }

    [PunRPC]
    void RPC_AliveCountUp(int _cnt, bool _isDeadMantal)
    {
        isDeadMental = _isDeadMantal;
        aliveCount = _cnt;
    }

    public void SecurityEnding()
    {
        // 경비원 혼자 남았을때 엔딩
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1 && isDeadMental)
        {
            if (catchCount >= patientCount/2) 
            {
                // 경비원 Normal 엔딩 호출
                Debug.Log("경비원 Normal");
                GameManager.Instance.SetEnding(UserRole.Security, UserEnding.WinEnding);
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                // 경비원 Lose 엔딩 호출
                Debug.Log("경비원 Lose");
                GameManager.Instance.SetEnding(UserRole.Security, UserEnding.LoseEnding);
                PhotonNetwork.LeaveRoom();
            }
        }
        else if(PhotonNetwork.CurrentRoom.PlayerCount <= 1 && deadCount >= 0 && isDeadMental)
        {
            GameManager.Instance.SetEnding(UserRole.Mental, UserEnding.WinEnding);
            PhotonNetwork.LeaveRoom();
        }
        else if(PhotonNetwork.CurrentRoom.PlayerCount <= 2 && !isDeadMental)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                SpawnGun();
            }
        }
    }

    [PunRPC]
    void SetMentalSpawn()
    {
        isDeadMental = false;
    }

    [PunRPC]
    void GameStart(int PlayerCnt)
    {
        patientCount = PlayerCnt;

        StartCoroutine(UnEnableLodding());
    }

    [PunRPC]
    void MentalWin()
    {
        GameManager.Instance.SetEnding(UserRole.Mental, UserEnding.WinEnding);

        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    void RemoveSpawnPlayerList(int idx)
    {
        if (idx >= patientSpawn.Length)
            return;
        patientSpawn = RemoveTransformAt(patientSpawn, idx);
    }

    IEnumerator UnEnableLodding()
    {
        yield return new WaitForSeconds(3.0f);

        GameManager.Instance.isGameStart = true;
        loadding.SetActive(false);

        foreach (GameObject _speaker in GameObject.FindGameObjectsWithTag("Speaker"))
        {
            _speaker.GetComponent<AudioSource>().volume = 1;
        }

        foreach (AudioSource _obj in FindObjectsOfType<AudioSource>())
        {
            _obj.volume = 1;
        }

        SoundManager.instance.audioSourceBGM.volume = 0;
    }
}