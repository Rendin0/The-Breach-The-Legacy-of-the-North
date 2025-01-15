
using R3;

public class InventorySlotViewModel
{
    private readonly InventorySlot _origin;

    public ReactiveProperty<string> ItemId { get; }
    public ReactiveProperty<int> Amount { get; }

    public InventorySlotViewModel(InventorySlot origin)
    {
        _origin = origin;

        ItemId = origin.ItemId;
        Amount = origin.Amount;
    }
}