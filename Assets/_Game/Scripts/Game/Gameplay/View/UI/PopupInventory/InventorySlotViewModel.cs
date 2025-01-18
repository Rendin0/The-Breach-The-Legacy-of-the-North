
using R3;
using System;

public class InventorySlotViewModel
{
    private readonly InventorySlot _origin;

    public ReactiveProperty<string> ItemId { get; }
    public ReactiveProperty<int> Amount { get; }

    public readonly Subject<InventorySlotViewModel> SelectRequested = new();

    public void RequestSelect()
    {
        SelectRequested.OnNext(this);
    }
    public InventorySlotViewModel(InventorySlot origin)
    {
        _origin = origin;

        ItemId = origin.ItemId;
        Amount = origin.Amount;
    }
}