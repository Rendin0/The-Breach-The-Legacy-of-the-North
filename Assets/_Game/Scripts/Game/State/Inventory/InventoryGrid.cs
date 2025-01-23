
using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;

public class InventoryGrid
{
    public readonly InventoryGridData Origin;

    public int OwnerId { get; }

    private List<InventorySlot> _equipment = new();
    public ObservableDictionary<EquipmentType, InventorySlot> Equipment { get; } = new();
    public Storage Storage; 

    public readonly Dictionary<string, ItemConfig> ItemsConfig = new();

    public InventoryGrid(InventoryGridData grid)
    {
        Origin = grid;
        OwnerId = grid.OwnerId;
        Storage = new Storage(Origin.Storage);

        foreach (var item in Origin.ItemsConfig.Items)
            ItemsConfig[item.ItemId] = item;

        for (int i = 0; i < Enum.GetValues(typeof(EquipmentType)).Length; i++)
        {
            var item = grid.Equipment[i];
            ItemEquipmentConfig cfg = ItemsConfig[item.ItemId] as ItemEquipmentConfig;
            var slot = new InventorySlot(item);
            Equipment[(EquipmentType)i] = slot;
            _equipment.Add(slot);
        }
        Equipment.ObserveAdd().Subscribe(e =>
        {
            _equipment.Add(e.Value.Value);

            Origin.Equipment.Add(e.Value.Value.Origin);
        });
        Equipment.ObserveRemove().Subscribe(e =>
        {
            throw new NotImplementedException();
        });
    }

}