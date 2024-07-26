using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[System.Flags]*/
/*public enum ItemType        // 아이템 유형
{
    /// <summary>
    /// NONE 타입은 아이템을 습득하기 위해 E키를 누른 경우, 인벤토리에 들어오지 않는다.
    /// 특별한 상호작용이 있는 오브젝트로 취급한다.
    /// </summary>
    /// 
    NONE                = 0b0,
    SKILL               = 0b1,

    // 장비 아이템
    // 장비 아이템 타입에서 추가되는 경우, 증가하는 값으로 추가한다.
    Equip_Knife         = 0b10,
    Equip_Gun           = 0b100,
    Equip_Radio         = 0b1000,

    // 소모품 아이템
    Used                = 0b10000,

    // 탈출 아이템
    Escape              = 0b100000,
}*/
namespace New
{
    [CreateAssetMenu(fileName = "Item", menuName = "Add Item/Item")]
    public class Item : ScriptableObject
    {
        [Header("고유한 아이템의 ID(중복불가)")]
        [SerializeField] private int _ItemID;
        /// <summary>
        /// 아이템의 고유 번호
        /// </summary>
        /// <value></value>

        public int ItemID
        {
            get
            {
                return _ItemID;
            }
        }

        [Header("사용(상호작용)이 가능한 아이템인가?")]
        [SerializeField] private bool _IsInteractivity;
        /// <summary>
        /// 사용(상호작용)이 가능한 아이템인가?
        /// </summary>
        /// <value></value>
        public bool IsInteractivity
        {
            get
            {
                return _IsInteractivity;
            }
        }

        [Header("아이템을 사용하면 사라지는가?")]
        [SerializeField] private bool _IsUsed;
        /// <summary>
        /// 아이템을 사용하면 한개씩 사라지는가?
        /// </summary>
        /// <value></value>
        public bool IsUsed
        {
            get
            {
                return _IsUsed;
            }
        }

        [Header("아이템을 사용시 쿨타임")]
        [SerializeField] private float _ItemCooltime = -1;
        /// <summary>
        /// 아이템의 쿨타임
        /// </summary>
        /// <value></value>
        public float Cooltime
        {
            get
            {
                return _ItemCooltime;
            }
        }

        [Header("아이템의 타입")]
        [SerializeField] private ItemType _ItemType;
        /// <summary>
        /// 아이템의 유형
        /// </summary>
        /// <value></value>
        public ItemType Type
        {
            get
            {
                return _ItemType;
            }
        }

        [Header("인벤토리에서 보여질 아이템의 이미지")]
        [SerializeField] private Sprite _ItemImage;
        public Sprite Image
        {
            get
            {
                return _ItemImage;
            }
        }

        [Header("씬에서 오브제그로 보여질 아이템의 프리팹")]
        [SerializeField] private GameObject _ItemPrefab;

        public GameObject ItemPrefab
        {
            get
            {
                return _ItemPrefab;
            }
        }
    }

}
