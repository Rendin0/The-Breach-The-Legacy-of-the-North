
using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumable,
    Quest
}

public enum ItemRarity
{
    Quest = -2,
    Nothing = -1,
    Gray = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3,
    Red = 4
}

public abstract class ItemConfig : ScriptableObject
{
    public abstract ItemType Type { get; }
    public ItemRarity Rarity;
    public string ItemId;
    [Min(1)] public int MaxStack = 1;
    [TextArea] public string Desription;
}

