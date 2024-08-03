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
        switch (item.itemType)
        {
            case ItemType.Skill:
            {
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
                        int currnetPlayerHp = transform.GetComponent<PlayerHp>().GetPlayerHp();
                        if (currnetPlayerHp > 1)
                        {
                            Debug.Log("현재 체력 " + transform.GetComponent<PlayerHp>().GetPlayerHp().ToString());
                            return false;
                        }
                        else
                        {

                            // 자신 회복
                            transform.GetComponent<PlayerHp>().Heal(1);
                            Debug.Log("현재 체력 " + transform.GetComponent<PlayerHp>().GetPlayerHp().ToString());
                            return true;
                        }
                    }

                    case "열쇠":
                    {
                        // 창문 방을 제외한 잠긴 문을 2초를 소모하여 열 수 있다.
                        // 창문 방 제외 조건 추가해야됨
                        float unlockTime = 2f;
                        if (!GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().IsLockDoor())
                        {
                            if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().CanDoorAction(unlockTime))
                            {
                                StartCoroutine(UseKey(unlockTime));
                            }
                            return true;
                        }

                         return false;
 
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

    IEnumerator UseKey(float time)               // 2초후 문 열림
    {
        yield return new WaitForSeconds(time);        
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().UnlockDoor();
    }
}

