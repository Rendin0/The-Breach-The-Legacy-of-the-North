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

    // True - получилось закинуть весь стак
    // False - либо получилось закинуть не весь стак, либо не получилось закинуть вовсе
    public bool Handle(CmdAddItem command)
    {
        var storage = _inventoriesService.GetInventory(command.InventoryId).Storage;


        var freeSlot = GetFirstFreeSlotIndex(storage, command.ItemId);
        if (freeSlot.Item1 == null)
            return false;

        // ≈сли есть свободный слот, но в нЄм не хватит места на весь стак
        if (freeSlot.Item2 < command.Amount)
        {
            //  ладЄм в уже свободный слот и пытаемс€ докинуть оставшеес€
            _commandProcessor.Process(new CmdAddItemInSlot(command.InventoryId, freeSlot.Item1, command.ItemId, freeSlot.Item2));
            command.Amount -= freeSlot.Item2;
            return _commandProcessor.Process(command);
        }

        // Ќайден свободный слот и в нЄм хватает места на весь стак
        return _commandProcessor.Process(new CmdAddItemInSlot(command.InventoryId, freeSlot.Item1, command.ItemId, command.Amount));
    }

    // »ндекс, кол-во свободного места
    private (InventorySlotViewModel, int) GetFirstFreeSlotIndex(StorageViewModel storage, string itemId)
    {

        if (storage != null)
        {
            // ѕроверка незаполненных слотов
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

            // ѕроверка пустых слотов
            foreach (var slot in storage.Slots)
                if (slot.ItemId.Value == ItemsIDs.Nothing)
                    return (slot, _itemsConfigMap[itemId].MaxStack);

        }

        return (null, -1);
    }
}