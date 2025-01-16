using R3;
using ObservableCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoriesService
{
    private readonly ICommandProcessor _commandProcessor;

    private readonly Dictionary<int, PopupInventoryViewModel> _inventoriesMap = new();
    private readonly ObservableList<PopupInventoryViewModel> _inventoryViewModels = new();

    public IObservableCollection<PopupInventoryViewModel> InventoryViewModels => _inventoryViewModels;

    public InventoriesService(IObservableCollection<InventoryGrid> inventories, ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;

        foreach (var inventory in  inventories)
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


    private void CreateInventoryViewModel(InventoryGrid inventory)
    {
        if (_inventoriesMap.ContainsKey(inventory.OwnerId))
        {
            Debug.LogError("Trying to add inventory to already existing one");
            return;
        }

        var viewModel = new PopupInventoryViewModel(inventory);
        _inventoriesMap[inventory.OwnerId] = viewModel;
        _inventoryViewModels.Add(viewModel);
    }
    private void RemoveInventoryViewModel(InventoryGrid inventory)
    {
        if (_inventoriesMap.TryGetValue(inventory.OwnerId, out var viewModel))
        {
            _inventoriesMap.Remove(inventory.OwnerId);
            _inventoryViewModels.Remove(viewModel);
        }

    }
}