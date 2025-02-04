using System.Collections.Generic;

public class CmdAddItemHandler : ICommandHandler<CmdAddItem>
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly InventoriesService _inventoriesService;
    private readonly Dictionary<string, ItemConfig> _itemsConfigMap = new();


    public CmdAddItemHandler(ICommandProcessor commandProcessor, ItemsConfig itemsConfig, InventoriesService inventoriesService)
    {
        _commandProcessor = commandProcessor;
        _inventoriesService = inventoriesService;

        foreach (var itemConfig in itemsConfig.Items)
        {
            _itemsConfigMap[itemConfig.ItemId] = itemConfig;
        }
    }

    // True - ���������� �������� ���� ����
    // False - ���� ���������� �������� �� ���� ����, ���� �� ���������� �������� �����
    public bool Handle(CmdAddItem command)
    {
        var storage = _inventoriesService.GetInventory(command.InventoryId).Storage;


        var freeSlot = GetFirstFreeSlotIndex(storage, command.ItemId);
        if (freeSlot.Item1 == null)
            return false;

        // ���� ���� ��������� ����, �� � �� �� ������ ����� �� ���� ����
        if (freeSlot.Item2 < command.Amount)
        {
            // ����� � ��� ��������� ���� � �������� �������� ����������
            _commandProcessor.Process(new CmdAddItemInSlot(command.InventoryId, freeSlot.Item1, command.ItemId, freeSlot.Item2));
            command.Amount -= freeSlot.Item2;
            return _commandProcessor.Process(command);
        }

        // ������ ��������� ���� � � �� ������� ����� �� ���� ����
        return _commandProcessor.Process(new CmdAddItemInSlot(command.InventoryId, freeSlot.Item1, command.ItemId, command.Amount));
    }

    // ������, ���-�� ���������� �����
    private (InventorySlotViewModel, int) GetFirstFreeSlotIndex(StorageViewModel storage, string itemId)
    {

        if (storage != null)
        {
            // �������� ������������� ������
            foreach (var slot in storage.Slots)
            {
                if (slot.ItemId.Value == itemId)
                {
                    var slotAmount = slot.Amount.Value;
                    var slotMaxStack = _itemsConfigMap[itemId].MaxStack;

                    if (slotAmount < slotMaxStack)
                        return (slot, slotMaxStack - slotAmount);
                }
            }

            // �������� ������ ������
            foreach (var slot in storage.Slots)
                if (slot.ItemId.Value == ItemsIDs.Nothing)
                    return (slot, _itemsConfigMap[itemId].MaxStack);

        }

        return (null, -1);
    }
}