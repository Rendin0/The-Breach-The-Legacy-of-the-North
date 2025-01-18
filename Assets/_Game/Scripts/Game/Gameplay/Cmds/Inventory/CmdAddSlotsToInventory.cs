
public class CmdAddSlotsToInventory : ICommand
{
    public readonly PopupInventoryViewModel Inventory;
    public readonly int Amount;

    public CmdAddSlotsToInventory(PopupInventoryViewModel inventory, int amount)
    {
        Amount = amount;
        Inventory = inventory;
    }
}