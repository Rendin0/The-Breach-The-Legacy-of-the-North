
public class CmdAddSlotsToInventoryHandler : ICommandHandler<CmdAddSlotsToInventory>
{
    public CmdAddSlotsToInventoryHandler()
    {
        
    }

    public bool Handle(CmdAddSlotsToInventory command)
    {
        var slots = command.Inventory.Origin.Slots;

        for (int i = 0; i < command.Amount; i++)
        {
            slots.Add(new InventorySlot(new InventorySlotData()));
        }

        return true;
    }
}