
using ObservableCollections;
using R3;
using UnityEngine;

public class InventoryGrid
{
    public  readonly InventoryGridData Origin;

    public int OwnerId { get; }
    public ObservableList<InventorySlot> Slots { get; } = new();

    public InventoryGrid(InventoryGridData grid)
    {
        Origin = grid;

        OwnerId = grid.OwnerId;
        grid.Slots.ForEach(i => Slots.Add(new InventorySlot(i)));

        Slots.ObserveAdd().Subscribe(i =>
        {
            var added = i.Value;
            Origin.Slots.Add(added.Origin);
        });

        Slots.ObserveRemove().Subscribe(i =>
        {
            // Возможны ошибки, но вообще удалять слоты не надо
            var removed = i.Value;
            Origin.Slots.Remove(removed.Origin);
        });
    }

}