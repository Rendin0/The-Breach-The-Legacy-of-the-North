
using UnityEngine;

[CreateAssetMenu(fileName = "ItemWeaponConfig_", menuName = "Game Config/Items/New Weapon Config")]

public class ItemWeaponConfig : ItemConfig
{
    public override ItemType Type => ItemType.Weapon;

    public float Damage;
}