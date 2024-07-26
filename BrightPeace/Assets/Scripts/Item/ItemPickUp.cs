using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using New;
/// <summary>
/// 아이템을 넣는 공간 프리팹에 컴포넌트로 추가하고 인스펙터에 아이템을 할당한다.
/// Item에 대한 정보를 전달하는 클래스
/// </summary>
/// 
public class ItemPickUp : MonoBehaviour
{
    [Header("해당 오브젝트에 할당되는 아이템")]
    [SerializeField] private ItemData _Item;

    /// <summary>
    /// 상호작용 가능한 객체가 가지고 있는 아이템
    /// </summary>
    public ItemData Item
    {
        get
        {
            return _Item;
        }
    }

}
