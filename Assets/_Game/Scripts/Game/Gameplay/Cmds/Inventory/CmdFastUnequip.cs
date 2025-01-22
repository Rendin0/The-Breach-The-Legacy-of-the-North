
public class CmdFastUnequip : ICommand
{
    public readonly PopupInventoryViewModel Inventory;

    public CmdFastUnequip(PopupInventoryViewModel inventory)
    {
        Inventory = inventory;
    }
}
