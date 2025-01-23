
public class CmdEquipItem : ICommand
{
    public readonly InventorySlotViewModel Item;
    public readonly EquipmentType Equipment;
    public readonly PopupInventoryViewModel Inventory;

    public CmdEquipItem(InventorySlotViewModel item, EquipmentType equipment, PopupInventoryViewModel inventory)
    {
        Item = item;
        Equipment = equipment;
        Inventory = inventory;
    }
}
