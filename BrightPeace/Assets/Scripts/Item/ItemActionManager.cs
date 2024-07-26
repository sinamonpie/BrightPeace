using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionManager : MonoBehaviour
{
    public static string _SkillMessage = "ActiveSkill";

    [Header("플레이어 아이템이 상호작용하는 객체")]
    [SerializeField] private GameObject[] _Object;

    public bool UseItem(ItemData item)
    {

        switch (item.itemType)
        {
            case ItemType.Skill:        // 스킬
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

            case ItemType.Used:         // 소모품
            {
                switch (item.itemName)
                {
                    case "구급약":
                    {
                        Debug.Log("포션 사용!");
                        int currnetPlayerHp = transform.GetComponent<PlayerHp>().currentHp;
                        if (currnetPlayerHp > 1)
                        {
                            Debug.Log("현재 체력 " + transform.GetComponent<PlayerHp>().currentHp.ToString());
                            break;
                        }

                        transform.GetComponent<PlayerHp>().currentHp += 1;
                        Debug.Log("현재 체력 " + transform.GetComponent<PlayerHp>().currentHp.ToString());
                        break;
                    }

                    case "열쇠":
                    {
                        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().CanDoorAction())
                        {
                            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActionController>().UnlockDoor();
                            Debug.Log("Use Key");
                            break;
                        }
                        else
                        {
                            // 문을 바라보고 쓰시오 라는 메세지 띄우기
                        }
                        break;
                    }

                    case "투시경":
                    {
                        float wallHackTime = 2f;
                        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrayScreen>().ApplyGrayScreen(wallHackTime);
                        GameObject.FindAnyObjectByType<WallHacker>().ApplyWallHack(wallHackTime);
                        break;
                    }

                    // 추가 예정


                }
                    break;
            }

        }

        return true;
    }

}

