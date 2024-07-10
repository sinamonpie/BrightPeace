using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData itemData;

    public virtual void UseItem()
    {
        //  일회성
        //  사용 효과 재정의
    }
}
