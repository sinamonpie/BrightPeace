using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using New;

/// <summary>
///  인벤토리 슬롯 하나를 담당
/// </summary>
public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Item _Item;    // 현재 아이템 인스턴스
    public Item Item
    {
        get
        {
            return _Item;
        }
    }

    [Header("해당 슬롯에 어떠한 타입만 들어올 수 있는지 타입 마스크")]
    [SerializeField] private ItemType _SlotMask;


    [Header("아이템 슬롯에 있는 UI 오브젝트")]
    [SerializeField] private Image _ItemImage;          //아이템의 이미지
    [SerializeField] private Image _CooltimeImage;      //아이템 쿨타임 이미지

    

    // 아이템 이미지의 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = _ItemImage.color;
        color.a = _alpha;
       _ItemImage.color = color;
    }

    /// <summary>
    /// mSlotMask에서 설정된 값에 따라 비트연산을한다.
    /// 현재 마스크값이 비트연산으로 0이 나온다면 현재 슬롯에 마스크가 일치하지 않는다는 뜻.
    /// 0이 아닌 수는 현재 비트위치(10진수로 1, 2, 4, 8)로 값이 나온다.
    /// </summary>
    public bool IsMask(Item item)
    {
        return ((int)item.Type & (int) _SlotMask) == 0 ? false : true;
    }

    // 인벤토리에 새로운 아이템 슬롯 추가
    public void Additem(Item item)
    {
        _Item = item;
        _ItemImage.sprite = _Item.Image;

        SetColor(1);
    }

    // 해당 슬롯의 아이템 슬롯 개수 업데이트
    // 해당 슬롯 하나 삭제
    public void ClearSlot()
    {
        _Item = null;
        _ItemImage.sprite = null;
        SetColor(0);
    }

    /// <summary>
    /// 외부에서 해당 슬롯을 대상으로 직접 사용하도록 호출
    /// </summary>
    public void UseItem()
    {
        if (_Item != null)  // 해당 슬롯 아이템이 null이면 return
        {
            // 사용이 불가능한 아이템이면 return
            if (!_Item.IsInteractivity)
                return;
            // 쿨타임이 0보다 큰경우 (현재 쿨타임이 돌고있는 경우 return

            // 아이템 사용 호츨
/*            if (!_ItemActionManager.UseItem(_Item))
            {
                return;
            }*/

            // 아이템의 쿨타임이 설정되어있으면 쿨타임 적용
/*            if(_Item.Cooltime > 0f)
            {
                ItemCooltimeManager.Instance.AddCooltimeQueue(_Item.ItemID, _Item.Cooltime);
            }*/

            // 장비 아이템을 사용한 경우
            if(_Item.Type >= ItemType.Equip)
            {

            }

            // 소모품 아이템을 사용한 경우 개수 줄임
            if (_Item.Type == ItemType.Used)
            {

            }

            // 아이템을 다쓴경우, UpdateSlotCount로 인해 _Item이 null이 되는 경우 UI 끄기
            if(_Item == null)
            {
             
            }
        }
    }



}
