using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 슬롯 등록 / 사용할 준비
/// 추상클래스로 작성 / 인벤토리 베이스 자체적으로 인스턴스 할 수 없음
/// </summary>

abstract public class InventoryBase : MonoBehaviour
{
    [SerializeField] protected GameObject _InventorySlotsParents;       // 슬롯을 담을 최상위 오브젝트
    protected InventorySlot[] _Slots;

    protected void Awake()
    {
       _Slots = _InventorySlotsParents.GetComponentsInChildren<InventorySlot>();
    }



}
