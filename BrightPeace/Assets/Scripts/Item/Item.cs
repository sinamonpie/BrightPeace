using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Older
{
    public class Item : MonoBehaviour
    {
        public ItemData itemData;
        public GameObject player;
        public virtual bool UseItem()
        {
            return true;
        }

        public virtual void Equip()
        {

        }

        public virtual void UnEquip()
        {

        }
    }

}