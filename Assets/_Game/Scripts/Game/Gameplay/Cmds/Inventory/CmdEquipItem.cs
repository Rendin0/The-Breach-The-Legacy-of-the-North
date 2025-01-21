
public class CmdEquipItem : ICommand
{
    public readonly int ItemIndex;
    public readonly EquipmentType EquipmentIndex;
    public readonly PopupInventoryViewModel Inventory;

    public CmdEquipItem(int itemIndex, EquipmentType equipmentIndex, PopupInventoryViewModel inventory)
    {
        ItemIndex = itemIndex;
        EquipmentIndex = equipmentIndex;
        Inventory = inventory;
    }
}
