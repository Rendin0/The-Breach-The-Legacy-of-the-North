
using R3;

public class InventorySlot
{
    public InventorySlotData Origin { get; }

    public ReactiveProperty<string> ItemId { get; }
    public ReactiveProperty<int> Amount { get; }


    public InventorySlot(InventorySlotData origin)
    {
        Origin = origin;

        ItemId = new ReactiveProperty<string>(origin.ItemId);
        Amount = new ReactiveProperty<int>(origin.Amount);

        ItemId.Skip(1).Subscribe(value => origin.ItemId = value);
        Amount.Skip(1).Subscribe(amount => origin.Amount = amount);
    }
}