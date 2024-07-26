
using UnityEngine;
using New;
/// <summary>
/// 아이템을 관리하고 보환할 인벤토리
/// </summary>
public class InventoryMain : InventoryBase
{
    new void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 특정 아이템 슬롯에 아이템을 등록시킨다.
    /// </summary>

    public void AcquireItem(Item item) 
    {

        // 빈 슬롯에 넣기
        for (int i = 0; i < _Slots.Length; i++)
        {
            if (_Slots[i].Item == null && _Slots[i].IsMask(item))
            {
                _Slots[i].Additem(item);
                return;
            }
        }

    }

    public InventorySlot[] GetAllItems()
    {
        return _Slots;
    }
}
