using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefab;
}
