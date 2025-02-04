
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CmdCreateInventoryHandler : ICommandHandler<CmdCreateInventory>
{
    private readonly GameStateProxy _gameState;
    private readonly ItemsConfig _itemsConfigs;

    public CmdCreateInventoryHandler(GameStateProxy gameState, ItemsConfig itemsConfigs)
    {
        _gameState = gameState;
        _itemsConfigs = itemsConfigs;
    }

    public bool Handle(CmdCreateInventory command)
    {
        if (_gameState.Creatures.FirstOrDefault(c => c.Id == command.OwnerId) == null)
        {
            Debug.LogError($"Entity with {command.OwnerId} id does not exist");
            return false;
        }

        if (_gameState.Inventories.FirstOrDefault(i => i.OwnerId == command.OwnerId) != null)
        {
            Debug.LogError($"Entity with {command.OwnerId} id aleady have an inventory");
            return false;
        }

        var storage = new StorageData()
        {
            Slots = new List<InventorySlotData>()
        };

        var inventoryData = new InventoryGridData()
        {
            OwnerId = command.OwnerId,
            Storage = storage,
            Equipment = new List<InventorySlotData>(),
        };
        for (int i = 0; i < command.Size; i++)
            inventoryData.Storage.Slots.Add(new InventorySlotData());

        for (int i = 0; i < Enum.GetValues(typeof(EquipmentType)).Length; i++)
            inventoryData.Equipment.Add(new InventorySlotData());

        var inventory = new InventoryGrid(inventoryData);
        _gameState.Inventories.Add(inventory);

        return true;
    }
}