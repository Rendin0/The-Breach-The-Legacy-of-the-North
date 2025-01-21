
using UnityEngine;

[CreateAssetMenu(fileName = "ItemWeaponConfig_", menuName = "Game Config/Items/New Weapon Config")]

public class ItemWeaponConfig : ItemEquipmentConfig
{
    public override ItemType Type => ItemType.Equipment;

    public float Damage;
}