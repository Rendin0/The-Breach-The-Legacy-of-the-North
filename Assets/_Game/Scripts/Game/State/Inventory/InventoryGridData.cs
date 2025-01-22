using System;
using System.Collections.Generic;

[Serializable]
public class InventoryGridData
{
    public int OwnerId;
    public List<InventorySlotData> Slots;
    public List<InventorySlotData> Equipment;
    public ItemsConfig ItemsConfig;
}