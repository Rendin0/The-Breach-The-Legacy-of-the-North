
public class CmdAddItemInSlot : ICommand
{
    public readonly int InventoryId;
    public readonly int SlotIndex;
    public string ItemId;
    public int Amount;

    public CmdAddItemInSlot(int inventoryId, int slotIndex, string itemId, int amount)
    {
        InventoryId = inventoryId;
        SlotIndex = slotIndex;
        ItemId = itemId;
        Amount = amount;
    }
}