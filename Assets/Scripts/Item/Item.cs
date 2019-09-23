using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int minStackSize;
    public ItemTypes itemTypes;
    public GameObject itemPrefab;
}
