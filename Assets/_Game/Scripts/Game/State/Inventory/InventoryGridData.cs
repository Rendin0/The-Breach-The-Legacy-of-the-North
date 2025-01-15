
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryGridData
{
    public int OwnerId;
    public List<InventorySlotData> Slots;
    public Vector2Int Size;
}