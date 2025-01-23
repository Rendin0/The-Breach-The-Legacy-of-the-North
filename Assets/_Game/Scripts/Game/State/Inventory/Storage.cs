using R3;
using ObservableCollections;

public class Storage
{
    public ObservableList<InventorySlot> Slots { get; } = new();
    public readonly StorageData Origin;

    public Storage(StorageData storage)
    {
        Origin = storage;

        storage.Slots.ForEach(i => Slots.Add(new InventorySlot(i)));
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