using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템을 사용했을때 해당 아이템 기능을 기재
/// </summary>
public class ItemActionManager : MonoBehaviour
{
    public static string _SkillMessage = "ActiveSkill";

    [Header("플레이어 아이템이 상호작용하는 객체")]
    [SerializeField] private GameObject[] _Object;

    public bool UseItem(ItemData item)
    {
        // 아이템은 장비 / 소모품 / 탈출아이템으로 구분
        switch (item.itemType)
        {
            case ItemType.Skill:
            {
                break;
            }

            case ItemType.Equip:
            {
                switch (item.itemName)
                {
                    case "총":
                    {
                        GameObject parentObject = GameObject.FindWithTag("MainCamera");
                        Transform childObject = parentObject.transform.GetChild(0);
                        GameObject AimManger = childObject.gameObject;
                        AimManger.SetActive(true);
                        break;
                    }

                    case "칼":
                    {
                        // 다른 플레이어의 체력을 1 깎고 해당 플레이어가 경비원이면 2초간 스턴시킨다.
                        // 칼이 인벤토리에 있다면 무조건 쥐게 되고 누구나 볼 수 있는 상태가 된다.
                        // 왼쪽 클릭시 플레이어의 이동 속도가 0.5초간 1.5배가 되고, 0.3초간 정면에 위치한 플레이어를 타격할 수 있게 된다.
                        // 실험체를 성공적으로 타격 시 다시 타격하기 까지 2.5초의 대기 시간이 있다.
                        // 실험체를 성공적으로 타격하면 이동 속도가 2.5초간 0.125배가 된다.
                        // 실험체 타격에 실패시 이동 속도가 1.5초간 0.25배가 된다.
                        GameObject parentObject = GameObject.FindWithTag("MainCamera");
                        Transform childObject = parentObject.transform.GetChild(1);
                        GameObject AimManger = childObject.gameObject;
                        AimManger.SetActive(true);
                        break;
                    }

                    case "무전기":
                    {
                        break;
                    }
                }
                break;
            }

            case ItemType.Used:
            {
                switch (item.itemName)
                {
                    case "구급약":
                    {
                        // 1.5배의 속도로 스스로와 타인을 치료할 수 있게 됩니다.
                        // 메디킷은 현재체력이 1일때만 사용가능하고 그외에는 사용할 수 없다.
                        int currnetPlayerHp = transform.GetComponent<PlayerHp>().currentHp;
                        if (currnetPlayerHp > 1)
                        {
                            Debug.Log("현재 체력 " + transform.GetComponent<PlayerHp>().currentHp.ToString());
                            return false;
                        }
                        else
                        {

                            // 자신 회복
                            transform.GetComponent<PlayerHp>().currentHp += 1;
                            Debug.Log("현재 체력 " + transform.GetComponent<PlayerHp>().currentHp.ToString());
                            return true;
                        }
                    }

                    case "열쇠":
                    {
                        // 창문 방을 제외한 잠긴 문을 2초를 소모하여 열 수 있다.
                        float unlockTime = 2f;
                        if (!GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().IsLockDoor())
                        {
                            if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().CanDoorAction(unlockTime))
                            {
                                StartCoroutine(UseKey(unlockTime));
                            }
                            return true;
                        }
                        else
                        {
                            // 문을 바라보고 쓰시오 라는 메세지 띄우기
                            return false;
                        }
                    }

                    case "투시경":
                    {
                        // 다른 모든 플레이어의 실루엣을 3초간 감지한다.
                        // 단, 캐비넷에 들어가있는 플레이어는 감지되지 않는다.
                        float wallHackTime = 3f;
                        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrayScreen>().ApplyGrayScreen(wallHackTime);
                        GameObject.FindAnyObjectByType<WallHacker>().ApplyWallHack(wallHackTime);
                        return true;
                    }

                }
                break;
            }

            case ItemType.Escape:
            {
                switch (item.itemName)
                {
                    case "락픽":
                    {
                        return true;
                    }
                    case "밸브":
                    {
                        return true;
                    }
                }
                break;
            }
        }
        return false;
    }
    
    IEnumerator UseMediKit()
    {
        yield return new WaitForSeconds(1);
    }

    IEnumerator UseKey(float time)               // 2초후 문 열림
    {
        yield return new WaitForSeconds(time);        
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().UnlockDoor();
    }
}

