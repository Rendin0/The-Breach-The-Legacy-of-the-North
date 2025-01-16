
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CmdAddItemHandler : ICommandHandler<CmdAddItem>
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly GameStateProxy _gameStateProxy;
    private readonly Dictionary<string, ItemConfig> _itemsConfigMap = new(); 


    public CmdAddItemHandler(ICommandProcessor commandProcessor, ItemsConfig itemsConfig, GameStateProxy gameStateProxy)
    {
        _commandProcessor = commandProcessor;
        _gameStateProxy = gameStateProxy;

        foreach (var itemConfig in itemsConfig.Items)
        {
            _itemsConfigMap[itemConfig.ItemId] = itemConfig;
        }
    }

    // True - ���������� �������� ���� ����
    // False - ���� ���������� �������� �� ���� ����, ���� �� ���������� �������� �����
    public bool Handle(CmdAddItem command)
    {
        var freeSlot = GetFirstFreeSlotIndex(command.InventoryId, command.ItemId);
        if (freeSlot.Item1 == -1)
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
    private (int, int) GetFirstFreeSlotIndex(int inventoryId, string itemId)
    {
        var inventory = _gameStateProxy.Inventories.FirstOrDefault(i => i.OwnerId == inventoryId);

        if (inventory != null)
        {
            // �������� ������������� ������
            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                var slot = inventory.Slots[i];
                if (slot.ItemId.Value == itemId)
                {
                    var slotAmount = slot.Amount.Value;
                    var slotMaxStack = _itemsConfigMap[itemId].MaxStack; 

                    if (slotAmount < slotMaxStack)
                        return (i,  slotMaxStack - slotAmount);
                }
            }

            // �������� ������ ������
            for (int i = 0; i < inventory.Slots.Count; i++)
                if (inventory.Slots[i].ItemId.Value == Items.Nothing)
                    return (i, _itemsConfigMap[itemId].MaxStack);
        }

        return (-1, -1);
    }
}