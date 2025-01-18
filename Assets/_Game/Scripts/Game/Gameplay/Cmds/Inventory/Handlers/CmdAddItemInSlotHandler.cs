
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CmdAddItemInSlotHandler : ICommandHandler<CmdAddItemInSlot>
{
    private readonly Dictionary<string, ItemConfig> _itemsConfigMap = new();
    private readonly GameStateProxy _gameState;

    public CmdAddItemInSlotHandler(ItemsConfig itemsConfig, GameStateProxy gameState)
    {
        _gameState = gameState;

        foreach (var item in itemsConfig.Items)
            _itemsConfigMap[item.ItemId] = item;
    }

    public bool Handle(CmdAddItemInSlot command)
    {
        var inventory = _gameState.Inventories.FirstOrDefault(i => i.OwnerId == command.InventoryId);

        if (inventory != null)
        {
            // Если слот не пустой
            if (inventory.Slots[command.SlotIndex].ItemId.Value != ItemsTypes.Nothing)
            {
                // Лежит другой предмет, нельзя стакать
                if (inventory.Slots[command.SlotIndex].ItemId.Value != command.ItemId)
                {
                    // Свап предмета из слота в команду
                    var tmpId = command.ItemId;
                    var tmpAmount = command.Amount;

                    command.ItemId = inventory.Slots[command.SlotIndex].ItemId.Value;
                    command.Amount = inventory.Slots[command.SlotIndex].Amount.Value;

                    inventory.Slots[command.SlotIndex].ItemId.OnNext(tmpId);
                    inventory.Slots[command.SlotIndex].Amount.OnNext(tmpAmount);
                    return false;
                }

                var maxStack = _itemsConfigMap[command.ItemId].MaxStack;
                var amountInSlot = inventory.Slots[command.SlotIndex].Amount.Value;
                // Лежит такой же предмет, но не хватает места
                if (amountInSlot + command.Amount > maxStack)
                {
                    inventory.Slots[command.SlotIndex].Amount.OnNext(maxStack);
                    command.Amount -= maxStack - amountInSlot; 

                    return false;
                }

                inventory.Slots[command.SlotIndex].Amount.OnNext(amountInSlot + command.Amount);
                return true;
            }

            inventory.Slots[command.SlotIndex].ItemId.OnNext(command.ItemId);
            inventory.Slots[command.SlotIndex].Amount.OnNext(command.Amount);
            return true;
        }

        Debug.LogError($"Couldnt find inventory with id {command.InventoryId}");


        return false;
    }
}