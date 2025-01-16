
public class CmdSortInventory : ICommand
{
    public readonly PopupInventoryViewModel Inventory;

    public CmdSortInventory(PopupInventoryViewModel inventory)
    {
        Inventory = inventory;
    }
}