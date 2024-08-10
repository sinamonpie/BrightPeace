using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Photon.Pun;
using System.Data;

public class ActionController : MonoBehaviourPun
{
    [Header("상호작용 거리")]
    [SerializeField]
    public float range;

    [Header("특정 상호작용 시간")]
    [SerializeField]
    private float holdTime = 3f;
    private float _holdTime = 0f;
    public RaycastHit hitInfo;
    public Ray ray;
    private bool isInvenFull;
    private GameObject currentLockDoor;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private TMP_Text alertText;
    public TMP_Text actionText;
    public Image actionImage;

    [SerializeField] Inventory inventory;
    [SerializeField] public GameObject player;

    public bool isRayItem;
    public bool canDoor = false;
    private bool isSetting = false;

    void Update()
    {
        if (isSetting)
        {
            CheckInteraction();
        }
    }

    void CheckInteraction()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.cyan);

        if (Physics.Raycast(ray, out hitInfo, range, layerMask))
        {

            if (hitInfo.transform.tag == "Item" && !PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                ItemInfoAppear();
                isRayItem = true;
            }

            if (hitInfo.transform.tag == "Door")
            {
                canDoor = true;
                DoorInfoAppear();
            }

            if(hitInfo.transform.tag == "ExitDoor")
            {
                if(hitInfo.transform.GetComponent<DoorController>().UseableDoor())
                {
                    canDoor = true;
                    DoorInfoAppear();
                }
            }

            if (hitInfo.transform.tag == "Ending" && !PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                canDoor = true;
                EndigInfoAppear();
            }

            if (hitInfo.transform.tag == "Cabinet" && !PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                canDoor = true;
                HideCabinetInfoAppear();
            }

            if (hitInfo.transform.tag == "Tank")
            {
                // 밸브를 달았을때
                if (hitInfo.transform.GetComponent<ValveTank>().GetValve())
                {
                    // 밸브가 활성화 안되었다면
                    if (!hitInfo.transform.GetComponent<ValveTank>().isUseValve)
                    {
                        // 실험체에 경우에만 밸브를 활성화 할 수 있음
                        if(player.GetComponent<PlayerState>().role != UserRole.Security)
                        {
                            if(hitInfo.transform.GetComponent<ValveTank>().usePlayer == null ||
                                hitInfo.transform.GetComponent<ValveTank>().usePlayer.GetPhotonView().ViewID == player.GetPhotonView().ViewID)
                            {
                                actionText.gameObject.SetActive(true);
                                actionText.text = "밸브 돌리기 " + "<color=yellow>" + "E Key" + "</color>";

                                if (Input.GetKey(KeyCode.E))
                                {
                                    SoundManager.instance.PlaySoundEffect("TurnValve");
                                    if (hitInfo.transform.GetComponent<ValveTank>().usePlayer == null)
                                        hitInfo.transform.GetComponent<ValveTank>().SetUsing(player.GetPhotonView().ViewID);

                                    player.GetComponent<PlayerController>().UnEnableMove();
                                    _holdTime += Time.deltaTime;

                                    actionImage.gameObject.SetActive(true);
                                    actionImage.fillAmount = 1f - (_holdTime / holdTime);

                                    // 특정 시간동안 키를 눌러야 활성화
                                    if (_holdTime >= holdTime)
                                    {
                                        hitInfo.transform.GetComponent<ValveTank>().NotUsing();
                                        player.GetComponent<PlayerController>().EnableMove();
                                        actionImage.gameObject.SetActive(false);
                                        actionText.gameObject.SetActive(false);
                                        _holdTime = 0f;
                                        UseValve(true);
                                    }
                                }
                                else
                                {
                                    hitInfo.transform.GetComponent<ValveTank>().NotUsing();

                                    player.GetComponent<PlayerController>().EnableMove();
                                    _holdTime = 0f;
                                    actionImage.fillAmount = 1f;
                                    actionImage.gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                player.GetComponent<PlayerController>().EnableMove();
                                actionText.gameObject.SetActive(true);
                                actionText.text = "다른사람이 벨브를 돌리고 있습니다. ";
                            }
                        }
                    }
                    else
                    {
                        // 밸브가 활성화 되어 있을때, 경비원 과 배신자는 밸브를 돌릴 수 있음
                        if (player.GetComponent<PlayerState>().role != UserRole.Patient)
                        {
                            if (hitInfo.transform.GetComponent<ValveTank>().usePlayer == null ||
                                hitInfo.transform.GetComponent<ValveTank>().usePlayer.GetPhotonView().ViewID == player.GetPhotonView().ViewID)
                            {
                                actionText.gameObject.SetActive(true);
                                actionText.text = "밸브 방해하기 " + "<color=yellow>" + "E Key" + "</color>";

                                if (Input.GetKey(KeyCode.E))
                                {
                                    SoundManager.instance.PlaySoundEffect("TurnValve");
                                    if (hitInfo.transform.GetComponent<ValveTank>().usePlayer == null)
                                        hitInfo.transform.GetComponent<ValveTank>().SetUsing(player.GetPhotonView().ViewID);

                                    player.GetComponent<PlayerController>().UnEnableMove();
                                    _holdTime += Time.deltaTime;

                                    actionImage.gameObject.SetActive(true);
                                    actionImage.fillAmount = 1f - (_holdTime / holdTime);

                                    // 특정 시간동안 키를 눌러야 활성화
                                    if (_holdTime >= holdTime)
                                    {
                                        hitInfo.transform.GetComponent<ValveTank>().NotUsing();
                                        player.GetComponent<PlayerController>().EnableMove();

                                        actionImage.gameObject.SetActive(false);
                                        actionText.gameObject.SetActive(false);
                                        _holdTime = 0f;
                                        UseValve(false);
                                    }
                                }
                                else
                                {
                                    hitInfo.transform.GetComponent<ValveTank>().NotUsing();

                                    player.GetComponent<PlayerController>().EnableMove();
                                    _holdTime = 0f;
                                    actionImage.fillAmount = 1f;
                                    actionImage.gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                player.GetComponent<PlayerController>().EnableMove();
                                actionText.gameObject.SetActive(true);
                                actionText.text = "다른사람이 벨브를 돌리고 있습니다. ";
                            }
                        }
                    }
                }
                else
                {
                    alertText.gameObject.SetActive(true);
                    alertText.text = "발전기를 사용하려면 밸브 아이템이 필요합니다. ";
                }

            }

            if (hitInfo.transform.tag == "FuseBox" && !PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                PuseBoxInfoAppear();
            }

            if (hitInfo.transform.tag == "EndingLobby" && !PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                EndigLobbyInfoAppear();
            }

        }
        else
        {
            InfoDisapper();
        }

    }
    void ItemInfoAppear()
    {
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().Item.itemName + " 획득하기 " + "<color=yellow>" + "E Key" + "</color>";
        PickupAction();
    }

    void DoorInfoAppear()
    {
        actionText.gameObject.SetActive(true);

        if (hitInfo.transform.GetComponent<DoorController>().isClose)
        {
            actionText.text = "문 열기 " + "<color=yellow>" + "E키" + "</color>";
        }
        else
        {
            actionText.text = "문 닫기 " + "<color=yellow>" + "E키" + "</color>";
        }
        DoorAction();
    }
    void InfoDisapper()
    {
        actionText.gameObject.SetActive(false);
        alertText.gameObject.SetActive(false);
        canDoor = false;
        isRayItem = false;
    }

    void PickupAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hitInfo.transform != null)
            {
                isInvenFull = inventory.AddItem(hitInfo.transform.GetComponent<ItemPickUp>().Item);
                if (isInvenFull)
                {
                    SoundManager.instance.PlaySoundEffect("ItemGet");
                    Debug.Log("획득하기 " + hitInfo.transform.GetComponent<ItemPickUp>().Item.itemName);
                    GameObject item = hitInfo.transform.gameObject;
                    item.GetComponent<ItemPickUp>().TakeItem();
                }
                else
                {
                    Debug.Log("Full Inventory");
                }
                InfoDisapper();
            }
        }
    }

    void DoorAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.GetComponent<DoorController>().UseableDoor())
                {
                    // 문 열리는 사운드 추가
                    hitInfo.transform.GetComponent<DoorController>().DoorControl();
                    Debug.Log(hitInfo);
                    InfoDisapper();
                }
                else
                {
                    // 문 잠긴 사운드 추가
                    alertText.gameObject.SetActive(true);
                    alertText.text = "문이 잠겼습니다.";
                    Invoke("DisAlert", 1f);
                }
            }
        }
    }


    public bool IsLockDoor()
    {
        if (hitInfo.transform.GetComponent<DoorController>().UseableDoor())
        {
            return true;        // 열린문
        }
        return false;           // 잠긴문
    }

    public bool CanDoorAction()
    {
        return canDoor;
    }
    public bool CanDoorAction(float time)
    {
        hitInfo.transform.GetComponent<DoorController>().SetDoorUI(time);
        currentLockDoor = hitInfo.transform.gameObject;
        return canDoor;
    }

    public void UnlockDoor()
    {
        currentLockDoor.GetComponent<DoorController>().UnlockDoor();
        currentLockDoor = null;
    }

    public void DisAlert()
    {
        alertText.gameObject.SetActive(false);
    }

    public void EndingAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.GetComponent<EscapeEnding>().EndigTriiger())
                {
                    if (GameManager.Instance.role == UserRole.Patient)
                    {
                        GameManager.Instance.SetEnding(UserEnding.WinEnding);
                        PhotonNetwork.LeaveRoom();
                        Debug.Log("엔딩조건 충족 / 엔딩씬 보여주기");
                    } 
                    else if (GameManager.Instance.role == UserRole.Mental)
                    {
                        GameManager.Instance.SetEnding(UserEnding.NomalEnding);
                        PhotonNetwork.LeaveRoom();
                        Debug.Log("엔딩조건 충족 / 엔딩씬 보여주기");
                    }
                }
                else
                {
                    alertText.gameObject.SetActive(true);
                    alertText.text = "문이 잠겼습니다.";
                    Invoke("DisAlert", 1f);
                }
            }
        }
    }

    public void EndigInfoAppear()
    {
        actionText.gameObject.SetActive(true);
        actionText.text = "<color=yellow>" + " 탈출하기 " + "E Key" + "</color>";
        EndingAction();
    }

    public void EndingLobbyAction()
    {
        if (hitInfo.transform != null && hitInfo.transform.GetComponent<EscapeEnding>())
        {
            if (hitInfo.transform.GetComponent<EscapeEnding>() != null && hitInfo.transform.GetComponent<EscapeEnding>().EndigTriiger())
            {
                if (GameManager.Instance.role == UserRole.Patient)
                {
                    GameManager.Instance.SetEnding(UserEnding.WinEnding);
                    PhotonNetwork.LeaveRoom();
                    Debug.Log("엔딩조건 충족 / 엔딩씬 보여주기");
                }
                else if (GameManager.Instance.role == UserRole.Mental)
                {
                    GameManager.Instance.SetEnding(UserEnding.NomalEnding);
                    PhotonNetwork.LeaveRoom();
                    Debug.Log("엔딩조건 충족 / 엔딩씬 보여주기");
                }
            }
            else
            {
                alertText.gameObject.SetActive(true);
                alertText.text = "퓨즈가 아직 다 활성화가 안되었습니다.";
                Invoke("DisAlert", 1f);
            }
        }
    }

    public void EndigLobbyInfoAppear()
    {
        actionText.gameObject.SetActive(true);
        actionText.text = "<color=yellow>" + " 카드키로 탈출하기 " + "E Key" + "</color>";
    }

    public void HideCabinetAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hitInfo.transform != null)
            {
                hitInfo.transform.GetComponent<CabinetController>().CabinetControl(player);
                if (hitInfo.transform.GetComponent<CabinetController>().HideInCabinet())
                {
                    // 캐비넷 들어가는 사운드 추가
                    // 플레이어 투시경에 안보이게 하는거 임시용
                    //player.GetComponent<ThirdPersonCharacter>()
                    player.GetComponent<PlayerState>().PlayerInCabinet();

                    // 7 = hiddenCharactor Layer
                    ChangePlayerLayer(player, 7);
                }
                else
                {
                    player.GetComponent<PlayerState>().PlayerInCabinet();

                    // 8 = Charactor Layer
                    ChangePlayerLayer(player, 8);
                }
            }
        }
    }

    public void HideCabinetInfoAppear()
    {
        actionText.gameObject.SetActive(true);
        if (hitInfo.transform.GetComponent<CabinetController>().HideInCabinet())
        {
            actionText.text = "캐비넷에서 나오기 " + "<color=yellow>" + "E키" + "</color>";
        }
        else
        {
            actionText.text = "캐비넷에 숨기 " + "<color=yellow>" + "E키" + "</color>";
        }
        HideCabinetAction();
    }

    void ChangePlayerLayer(GameObject player, int toLayer)
    {
        player.layer = toLayer;

        foreach (Transform child in player.transform)
        {
            if (child != null)
            {
                ChangePlayerLayer(child.gameObject, toLayer);
            }
        }
    }


    public void SetPlayer()
    {
        player = transform.root.gameObject;
        inventory = null;

        if (!PhotonNetwork.IsMasterClient && photonView.IsMine)
        {
            inventory = player.GetComponent<Inventory>();
            if(player.GetComponent<PlayerState>().role == UserRole.Patient)
            {
                holdTime = 6f;
            }
            else
            {
                // 배신자는 3초임
                holdTime = 3f;
            }
        }

        isSetting = true;
    }


    // 매개변수 true이면 실험체, false면 배신자, 경비원
    void UseValve(bool isPartient)
    {
        GameObject tank = hitInfo.transform.gameObject;
        tank.GetComponent<ValveTank>().OpenTheGate(isPartient);
    }

    IEnumerator TextAlert()
    {
        alertText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        alertText.gameObject.SetActive(false);
    }

    IEnumerator KeyDownUI(float holdtime)
    {
        actionImage.gameObject.SetActive(true);

        while (holdTime > holdtime)
        {
            actionImage.fillAmount = 1f - (holdTime / holdtime);
        }

        yield return new WaitForFixedUpdate();

        actionImage.gameObject.SetActive(false);
        actionImage.fillAmount = 1f;
    }

    public void UseLockPick()
    {
        if(currentLockDoor != hitInfo.transform.gameObject)
        {
            GetComponent<PlayerController>().UnEnableMove();
            currentLockDoor = hitInfo.transform.gameObject;
            StartCoroutine(LockPickAlert(120f, currentLockDoor.GetComponent<DoorController>()));
        }
    }
    
    public void StopLockPick()
    {
        GetComponent<PlayerController>().EnableMove();
    }

    IEnumerator LockPickAlert(float time, DoorController door)
    {
        while (time > 0)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            alertText.text = string.Format("문 따는 중...{0:0}:{1:00}", minutes, seconds);
            alertText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }
        alertText.gameObject.SetActive(false);

        door.UnlockDoor();
    }

    public void PuseBoxInfoAppear()
    {
        actionText.gameObject.SetActive(true);
        actionText.text = "퓨즈 넣기 " + "<color=yellow>" + "E키" + "</color>";
    }
}
