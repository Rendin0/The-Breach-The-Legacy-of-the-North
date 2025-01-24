using ObservableCollections;
using R3;
using System.Collections.Generic;
using UnityEngine;

public class InventoriesService
{
    private readonly ICommandProcessor _commandProcessor;

    private readonly Dictionary<int, PopupInventoryViewModel> _inventoriesMap = new();
    private readonly ObservableList<PopupInventoryViewModel> _inventoryViewModels = new();

    public readonly Dictionary<string, ItemConfig> ItemsConfig = new();

    public IObservableCollection<PopupInventoryViewModel> InventoryViewModels => _inventoryViewModels;

    public InventoriesService(IObservableCollection<InventoryGrid> inventories, ItemsConfig itemsConfig, ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;


        foreach (var item in itemsConfig.Items)
        {
            ItemsConfig[item.ItemId] = item;
        }

        foreach (var inventory in inventories)
        {
            CreateInventoryViewModel(inventory);
        }

        inventories.ObserveAdd().Subscribe(i =>
        {
            CreateInventoryViewModel(i.Value);
        });

        inventories.ObserveRemove().Subscribe(i =>
        {
            RemoveInventoryViewModel(i.Value);
        });
    }

    public bool CreateInventory(int ownerId, int size)
    {
        var command = new CmdCreateInventory(ownerId, size);
        var result = _commandProcessor.Process(command);
        return result;
    }

    public PopupInventoryViewModel GetInventory(int ownerId)
    {
        return _inventoriesMap[ownerId];
    }

    private bool AddItemInInventorySlot(CmdAddItemInSlot cmd)
    {
        return _commandProcessor.Process(cmd);
    }

    public bool AddItemInInventorySlot(InventorySlotViewModel slot, int ownerId, string itemId, int amount = 1)
    {
        var command = new CmdAddItemInSlot(ownerId, slot, itemId, amount);
        var result = _commandProcessor.Process(command);
        return result;
    }
    public bool AddItemInInventory(int ownerId, string itemId, int amount = 1)
    {
        var command = new CmdAddItem(ownerId, itemId, amount);
        var result = _commandProcessor.Process(command);
        return result;
    }

    private void CreateInventoryViewModel(InventoryGrid inventory)
    {
        if (_inventoriesMap.ContainsKey(inventory.OwnerId))
        {
            Debug.LogError("CreateInventoryViewModel: Trying to add inventory to already existing one");
            return;
        }
        var viewModel = new PopupInventoryViewModel(inventory, this);

        _inventoriesMap[inventory.OwnerId] = viewModel;
        _inventoryViewModels.Add(viewModel);
    }

    public bool SwapEquipment(InventorySlotViewModel item, EquipmentType slot, PopupInventoryViewModel viewModel)
    {
        var cmd = new CmdEquipItem(item, slot, viewModel);
        return _commandProcessor.Process(cmd);
    }
    public void SwapSlots(InventorySlotViewModel prev, InventorySlotViewModel curr, PopupInventoryViewModel viewModel)
    {
        var cmd = new CmdAddItemInSlot(viewModel.OwnerId, curr, prev.ItemId.Value,
            prev.Amount.Value);
        prev.Amount.OnNext(0);
        prev.ItemId.OnNext(ItemsIDs.Nothing);

        // Не получилось полостью закинуть, в команде хранится тип и кол-во предмета, кидаем их в предыдущий слот
        if (!AddItemInInventorySlot(cmd))
        {
            AddItemInInventorySlot(prev, viewModel.OwnerId, cmd.ItemId, cmd.Amount);
        }
    }

    public bool AddSlotsToInventory(int inventoryId, int amount)
    {
        if (_inventoriesMap.TryGetValue(inventoryId, out var viewModel))
        {
            var cmd = new CmdAddSlotsToInventory(viewModel, amount);
            var result = _commandProcessor.Process(cmd);
            return result;
        }

        Debug.LogError($"AddSlotsToInventory: Couldnt find inventory with id {inventoryId}");
        return false;
    }

    private void RemoveInventoryViewModel(InventoryGrid inventory)
    {
        if (_inventoriesMap.TryGetValue(inventory.OwnerId, out var viewModel))
        {
            _inventoriesMap.Remove(inventory.OwnerId);
            _inventoryViewModels.Remove(viewModel);
        }

    }

    public bool SortInventory(int ownerId)
    {
        var cmd = new CmdSortStorage(_inventoriesMap[ownerId].Storage);
        var result = _commandProcessor.Process(cmd);
        return result;
    }

    public bool FastUnequip(PopupInventoryViewModel popupInventoryViewModel)
    {
        var cmd = new CmdFastUnequip(popupInventoryViewModel);
        return _commandProcessor.Process(cmd);
    }
}