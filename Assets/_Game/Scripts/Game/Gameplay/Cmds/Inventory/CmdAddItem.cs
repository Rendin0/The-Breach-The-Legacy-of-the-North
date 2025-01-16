
public class CmdAddItem : ICommand
{
    public readonly int InventoryId;
    public readonly string ItemId;
    public int Amount;

    public CmdAddItem(int inventoryId, string itemId, int amount)
    {
        InventoryId = inventoryId;
        ItemId = itemId;
        Amount = amount;
    }
}