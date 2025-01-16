
using UnityEngine;

public enum ItemType
{
    Weapon,
    Consumable,
    Quest
}

public enum ItemRarity
{
    Gray,
    Green,
    Blue,
    Yellow,
    Red
}

public abstract class ItemConfig : ScriptableObject
{
    public abstract ItemType Type { get; }
    public ItemRarity Rarity;
    public string ItemId;
    [Min(1)] public int MaxStack = 1;
}

