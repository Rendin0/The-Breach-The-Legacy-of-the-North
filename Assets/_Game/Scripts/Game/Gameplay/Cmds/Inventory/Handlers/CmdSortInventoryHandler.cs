
using System;
using System.Collections.Generic;
using System.Linq;

public class CmdSortInventoryHandler : ICommandHandler<CmdSortInventory>
{
    private readonly GameStateProxy _gameStateProxy;
    private readonly Dictionary<string, ItemConfig> _itemsConfigMap = new();

    public CmdSortInventoryHandler(GameStateProxy gameStateProxy, ItemsConfig itemsConfig)
    {
        foreach(var itemConfig in itemsConfig.Items)
            _itemsConfigMap[itemConfig.ItemId] = itemConfig;

        _gameStateProxy = gameStateProxy;
    }

    public bool Handle(CmdSortInventory command)
    {
        var inventory = command.Inventory;

        StackItems(inventory.Slots);

        List<(string, int)> items = new();

        foreach (var slot in inventory.Slots)
        {
            items.Add((slot.ItemId.Value, slot.Amount.Value));
        }

        items = items.OrderByDescending(i => _itemsConfigMap[i.Item1].Rarity).ThenByDescending(i => i.Item1).ToList();

        for (int i = 0; i < items.Count; i++)
        {
            inventory.Slots[i].ItemId.OnNext(items[i].Item1);
            inventory.Slots[i].Amount.OnNext(items[i].Item2);
        }

        return true;
    }
    private void StackItems(List<InventorySlotViewModel> inventory)
    {
        foreach (var comparedSlot in inventory)
        {
            if (comparedSlot.ItemId.Value == ItemsTypes.Nothing)
                continue;

            foreach (var slot in inventory)
            {
                if (comparedSlot.ItemId.Value != slot.ItemId.Value)
                    continue;

                var maxStack = _itemsConfigMap[comparedSlot.ItemId.Value].MaxStack;

                if (slot.Amount.Value >= maxStack)
                    continue;

                if (slot.Amount.Value + comparedSlot.Amount.Value > maxStack)
                {
                    // ������ ���� � ����� �� ���������, �������� ���������� ����
                    comparedSlot.Amount.OnNext(comparedSlot.Amount.Value - (maxStack - slot.Amount.Value));
                    slot.Amount.OnNext(maxStack);
                }
                else
                {
                    // ������ ���� � ����� �� ���������, ���������� ���� ����
                    slot.Amount.OnNext(slot.Amount.Value + comparedSlot.Amount.Value);
                    comparedSlot.Amount.OnNext(0);
                    comparedSlot.ItemId.OnNext(ItemsTypes.Nothing);
                }
            }
        }
    }
}