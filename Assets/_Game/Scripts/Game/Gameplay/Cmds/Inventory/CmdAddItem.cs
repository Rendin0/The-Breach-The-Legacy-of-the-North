
public class CmdAddItem : ICommand
{
    public readonly string InventoryId;
    public readonly string ItemId;
    public readonly int Amount;

    public CmdAddItem(string inventoryId, string itemId, int amount)
    {
        InventoryId = inventoryId;
        ItemId = itemId;
        Amount = amount;
    }
}