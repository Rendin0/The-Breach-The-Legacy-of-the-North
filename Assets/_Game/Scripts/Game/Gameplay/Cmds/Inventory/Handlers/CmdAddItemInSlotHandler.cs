
using System.Collections.Generic;
using UnityEngine;

public class CmdAddItemInSlotHandler : ICommandHandler<CmdAddItemInSlot>
{
    private readonly Dictionary<string, ItemConfig> _itemsConfigMap = new();
    private readonly InventoriesService _inventoriesService;

    public CmdAddItemInSlotHandler(ItemsConfig itemsConfig, InventoriesService inventoriesService)
    {
        _inventoriesService = inventoriesService;

        foreach (var item in itemsConfig.Items)
            _itemsConfigMap[item.ItemId] = item;
    }

    public bool Handle(CmdAddItemInSlot command)
    {
        // Если инвентарь существует
        if (_inventoriesService.GetInventory(command.InventoryId) != null)
        {
            // Если слот не пустой
            if (command.Slot.ItemId.Value != ItemsIDs.Nothing)
            {
                // Лежит другой предмет, нельзя стакать
                if (command.Slot.ItemId.Value != command.ItemId)
                {
                    // Свап предмета из слота в команду
                    var tmpId = command.ItemId;
                    var tmpAmount = command.Amount;

                    command.ItemId = command.Slot.ItemId.Value;
                    command.Amount = command.Slot.Amount.Value;

                    command.Slot.ItemId.OnNext(tmpId);
                    command.Slot.Amount.OnNext(tmpAmount);
                    command.Slot.ItemDescription = _itemsConfigMap[command.Slot.ItemId.Value].Desription;
                    return false;
                }

                var maxStack = _itemsConfigMap[command.ItemId].MaxStack;
                var amountInSlot = command.Slot.Amount.Value;
                // Лежит такой же предмет, но не хватает места
                if (amountInSlot + command.Amount > maxStack)
                {
                    command.Slot.Amount.OnNext(maxStack);
                    command.Amount -= maxStack - amountInSlot;

                    return false;
                }

                command.Slot.Amount.OnNext(amountInSlot + command.Amount);
                return true;
            }

            command.Slot.ItemId.OnNext(command.ItemId);
            command.Slot.Amount.OnNext(command.Amount);
            command.Slot.ItemDescription = _itemsConfigMap[command.Slot.ItemId.Value].Desription;
            return true;
        }

        Debug.LogError($"Couldnt find inventory with id {command.InventoryId}");


        return false;
    }
}