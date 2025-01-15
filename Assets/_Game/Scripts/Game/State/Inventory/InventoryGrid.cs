
using ObservableCollections;
using R3;
using UnityEngine;

public class InventoryGrid
{
    private readonly InventoryGridData _origin;

    public int OwnerId { get; }
    public ObservableList<InventorySlot> Slots { get; } = new();
    public ReactiveProperty<Vector2Int> Size { get; }

    public InventoryGrid(InventoryGridData grid)
    {
        _origin = grid;

        OwnerId = grid.OwnerId;
        grid.Slots.ForEach(i => Slots.Add(new InventorySlot(i)));
        Size = new ReactiveProperty<Vector2Int>(grid.Size);

        Slots.ObserveAdd().Subscribe(i =>
        {
            var added = i.Value;
            _origin.Slots.Add(added.Origin);
        });

        Slots.ObserveRemove().Subscribe(i =>
        {
            // Возможны ошибки, но вообще удалять слоты не надо
            var removed = i.Value;
            _origin.Slots.Remove(removed.Origin);
        });
    }

}