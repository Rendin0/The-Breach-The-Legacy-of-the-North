
public class CmdAddItemInSlot : ICommand
{
    public readonly int InventoryId;
    public readonly InventorySlotViewModel Slot;
    public string ItemId;
    public int Amount;

    public CmdAddItemInSlot(int inventoryId, InventorySlotViewModel slot, string itemId, int amount)
    {
        InventoryId = inventoryId;
        Slot = slot;
        ItemId = itemId;
        Amount = amount;
    }
}