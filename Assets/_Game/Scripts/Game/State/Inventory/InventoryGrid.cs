
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

    public InventoryGrid(InventoryGridData grid)
    {
        Origin = grid;
        OwnerId = grid.OwnerId;
        Storage = new Storage(Origin.Storage);

        for (int i = 0; i < Enum.GetValues(typeof(EquipmentType)).Length; i++)
        {
            var item = grid.Equipment[i];
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