using System;
using System.Collections.Generic;

[Serializable]
public class InventoryGridData
{
    public int OwnerId;
    public StorageData Storage;
    public List<InventorySlotData> Equipment;
    public ItemsConfig ItemsConfig;
}