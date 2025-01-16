
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

        var slots = inventory.Slots;

        StackItems(inventory.Slots);
        SortById(inventory);
        SortByRarity(inventory);

        return true;
    }

    private void SortById(PopupInventoryViewModel inventory)
    {
        // Compare возращает меньше нул€, если перва€ меньше второй
        QuickSort(inventory.Slots, (a, b) => { return string.Compare(a.ItemId.Value, b.ItemId.Value) > 0; });
    }
    private void SortByRarity(PopupInventoryViewModel inventory) 
    {
        QuickSort(inventory.Slots, (a, b) => { return _itemsConfigMap[a.ItemId.Value].Rarity > _itemsConfigMap[b.ItemId.Value].Rarity; });
    }

    private void StackItems(List<InventorySlotViewModel> inventory)
    {
        foreach(var comparedSlot in inventory)
        {
            if (comparedSlot.ItemId.Value == "")
                continue;

            foreach(var slot in inventory)
            {
                if (comparedSlot.ItemId.Value != slot.ItemId.Value)
                    continue;

                var maxStack = _itemsConfigMap[slot.ItemId.Value].MaxStack;

                if (slot.Amount.Value >= maxStack)
                    continue;

                if (slot.Amount.Value + comparedSlot.Amount.Value > maxStack)
                {
                    // Ќайден слот с таким же предметом, частично поместилс€ стак
                    comparedSlot.Amount.OnNext(comparedSlot.Amount.Value - (maxStack - slot.Amount.Value));
                    slot.Amount.OnNext(maxStack);
                }
                else
                {
                    // Ќайден слот с таким же предметом, поместилс€ весь стак
                    slot.Amount.OnNext(slot.Amount.Value + comparedSlot.Amount.Value);
                    comparedSlot.Amount.OnNext(0);
                    comparedSlot.ItemId.OnNext("");
                }
            }
        }

    }
    private void Swap(InventorySlotViewModel x, InventorySlotViewModel y)
    {
        string tmpId = x.ItemId.Value;
        int tmpAmount = x.Amount.Value;
        x.ItemId.OnNext(y.ItemId.Value);
        x.Amount.OnNext(y.Amount.Value);

        y.ItemId.OnNext(tmpId);
        y.Amount.OnNext(tmpAmount);
    }

    //метод возвращающий индекс опорного элемента
    private int Partition(List<InventorySlotViewModel> array, int minIndex, int maxIndex,
        Func<InventorySlotViewModel, InventorySlotViewModel, bool> Comparator)
    {
        var pivot = minIndex - 1;
        for (var i = minIndex; i < maxIndex; i++)
        {
            if (Comparator(array[i], array[maxIndex]))
            {
                pivot++;
                Swap(array[pivot], array[i]);
            }
        }

        pivot++;
        Swap(array[pivot], array[maxIndex]);
        return pivot;
    }

    //быстра€ сортировка
    private List<InventorySlotViewModel> QuickSort(List<InventorySlotViewModel> array, int minIndex, int maxIndex,
        Func<InventorySlotViewModel, InventorySlotViewModel, bool> Comparator)
    {
        if (minIndex >= maxIndex)
        {
            return array;
        }

        var pivotIndex = Partition(array, minIndex, maxIndex, Comparator);
        QuickSort(array, minIndex, pivotIndex - 1, Comparator);
        QuickSort(array, pivotIndex + 1, maxIndex, Comparator);

        return array;
    }

    private List<InventorySlotViewModel> QuickSort(List<InventorySlotViewModel> array,
        Func<InventorySlotViewModel, InventorySlotViewModel, bool> Comparator)
    {
        return QuickSort(array, 0, array.Count - 1, Comparator);
    }
}