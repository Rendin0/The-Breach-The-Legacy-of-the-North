
using UnityEngine;

[CreateAssetMenu(fileName = "ItemConsumableConfig_", menuName = "Game Config/Items/New Consumable Config")]

public class ItemConsumableConfig : ItemConfig
{
    public override ItemType Type => ItemType.Consumable;
}

