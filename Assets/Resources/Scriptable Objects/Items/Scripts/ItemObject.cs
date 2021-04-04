using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory
{
    Food,
    Weapon,
    Armor,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    public GameObject inventoryDisplayPrefab;
    public ItemCategory type;
    [TextArea(15, 20)]
    public string description;
}
