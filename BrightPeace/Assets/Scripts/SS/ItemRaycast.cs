using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using New;
using TMPro;
/// <summary>
/// 씬 내의 아이템(또는 정적 물체)에 다가가면 해당 아이템을 줍거나, 상호작용 할 수 있도록 해주는 스크립트
/// </summary>
public class ItemRaycast : MonoBehaviour
{
    /// <summary>
    /// 레이캐스트 된 아이템
    /// </summary>
    private RaycastHit mHit;
    private Ray ray;

    [Header("아이템 인식 사거리")]
    [SerializeField] private float _RayDistance;
    private bool _IsPickupActive = false;  // 아이템 습득이 가능한가?
    private ItemPickUp _CurrentItem;       // 레이케스트 현재 등록된 아이템

    [SerializeField] private LayerMask _LayerMask;
    [SerializeField] private InventoryMain _Inventory;

    // 아이템 상호작용 가능시 보여질 텍스트
    [SerializeField] private TMP_Text actionText;

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        if (_IsPickupActive)
        {
            TryPickItem();
        }
    }

    /// <summary>
    /// 아이템을 주울 수 있는지 확인한다.
    /// </summary>
    void TryPickItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            /*if(_CurrentItem.Item.Type > ItemType.NONE)*/
            {
                // 현재 인벤토리 아이템 가져오기
                InventorySlot[] allItems = _Inventory.GetAllItems();

                int i = 0;
                for (; i < allItems.Length; i++)
                {
                    //현재 아이템 칸이 null이라면 주울 수 있는 상태
                    if (allItems[i].Item == null) { break; }

                }
                //모든 칸이 null이 아니고, 중첩이 불가능하면 주울 수 없음
                if (i == allItems.Length) { return; }

                //아이템 줍는 효과음 재생
            }

/*            TryPickUp();*/
            ItemInfoDisappear();

        }
    }

    /// <summary>
    /// 레이캐스트를 이용하여 아이템을 확인한다.
    /// </summary>
    private void CheckItem()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out mHit, _RayDistance, _LayerMask))
        {
            // 레이캐스트 결과의 태그가 아이템이라면?
            if (mHit.transform.tag == "Item")
            {
                // 아이템 얻어오기 및 정보 호출
                _CurrentItem = mHit.transform.GetComponent<ItemPickUp>();
                actionText.gameObject.SetActive(true);
                actionText.text = "드랍 E 키";
                // 아이템 줍기 가능
                _IsPickupActive = true;

                return;
            }
            //레이캐스트 닿았을 때, 아이템이 아닌경우에는 비활성화
            else
            {
                ItemInfoDisappear();
            }
        }
        //레이캐스트 결과가 없으면 비활성화
        else
        {
            ItemInfoDisappear();
        }
    }

    /// <summary>
    /// 아이템 정보 보여주기를 비활성화 한다.
    /// </summary>
    private void ItemInfoDisappear()
    {
        // 픽업 비활성화
        _IsPickupActive = false;
        // UI 텍스트 제거
        actionText.gameObject.SetActive(false);
        // 현재 아이템은 null
        _CurrentItem = null;
    }


    /// <summary>
    /// 아이템을 습득한다.
    /// </summary>
/*    private void TryPickUp()
    {
        if (_IsPickupActive)
        {
            if (_CurrentItem.Item.Type != ItemType.NONE)
            {
                _Inventory.AcquireItem(_CurrentItem.Item);
                Destroy(_CurrentItem.gameObject);
            }

            ItemInfoDisappear();
        }
    }*/

}
