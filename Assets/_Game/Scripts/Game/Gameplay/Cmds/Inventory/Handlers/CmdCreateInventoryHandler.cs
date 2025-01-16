
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CmdCreateInventoryHandler : ICommandHandler<CmdCreateInventory>
{
    private readonly GameStateProxy _gameState;

    public CmdCreateInventoryHandler(GameStateProxy gameState)
    {
        _gameState = gameState;
    }

    public bool Handle(CmdCreateInventory command)
    {
        var inventory = _gameState.Inventories.FirstOrDefault(i => i.OwnerId == command.OwnerId);

        if (inventory == null)
        {
            var inventoryData = new InventoryGridData()
            {
                OwnerId = command.OwnerId,
                Slots = new List<InventorySlotData>()
            };
            for (int i = 0; i < command.Size; i++)
                inventoryData.Slots.Add(new InventorySlotData());

            inventory = new InventoryGrid(inventoryData);
            _gameState.Inventories.Add(inventory);

            return true;
        }

        Debug.LogError($"Entity with {command.OwnerId} id aleady have an inventory");
        return false;
    }
}