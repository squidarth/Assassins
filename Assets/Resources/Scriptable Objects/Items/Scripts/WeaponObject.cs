using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName ="Inventory System/Items/Weapons")]
public class WeaponObject : ItemObject
{
    // Start is called before the first frame update
    public double hitChance;
    public GameObject displayPrefab;
    public void Awake()
    {
        type = ItemCategory.Weapon;    
    }
}
