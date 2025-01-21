
using System;
using UnityEngine;

[Serializable]
public enum EquipmentType
{
    Helmet = 0,
    Weapon = 1,
    Shoulder = 2,
    Chest = 3,
    Pants = 4,
    Belt = 5,
    Boots = 6,
    Rune = 7
}

[CreateAssetMenu(fileName = "ItemEquipmentConfig_", menuName = "Game Config/Items/New Equipment Config")]
public class ItemEquipmentConfig : ItemConfig
{
    public override ItemType Type => ItemType.Equipment;

    public EquipmentType EquipmentType;
}
