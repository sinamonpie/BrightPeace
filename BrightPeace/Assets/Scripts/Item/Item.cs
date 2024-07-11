using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;

    public virtual void UseItem()
    {
        Destroy(gameObject);
        //  사용 효과 재정의
    }
}
