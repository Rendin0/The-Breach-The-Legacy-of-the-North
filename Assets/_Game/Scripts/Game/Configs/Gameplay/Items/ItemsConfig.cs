
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsConfig", menuName = "Game Config/New Items Config")]
public class ItemsConfig : ScriptableObject
{
    public List<ItemConfig> Items;
}

