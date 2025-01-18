using R3;
using ObservableCollections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupInventoryViewModel : WindowViewModel
{
    public GameplayUIManager UIManager;

    public override string Id => "PopupInventory";
    public int OwnerId;

    public List<InventorySlotViewModel> Slots { get; } = new();
    public Subject<int> SelectedChanged = new();

    private int currSeleted = -1;
    private readonly InventoriesService _service;

    public PopupInventoryViewModel(InventoryGrid origin, InventoriesService service)
    {
        foreach (var slot in origin.Slots)
        {
            CreateSlotViewModel(slot);
        }

        origin.Slots.ObserveAdd().Subscribe(s =>
        {
            CreateSlotViewModel(s.Value);
        });

        origin.Slots.ObserveRemove().Subscribe(s =>
        {
            RemoveSlotViewModel(s.Value);
        });

        OwnerId = origin.OwnerId;
        _service = service;
    }

    private void CreateSlotViewModel(InventorySlot origin)
    {
        var viewModel = new InventorySlotViewModel(origin);
        int index = Slots.Count;
        viewModel.SelectRequested.Subscribe(s =>
        {
            int tmpSelected = currSeleted;
            currSeleted = index;

            if (currSeleted == tmpSelected)
            {
                tmpSelected = currSeleted = -1;
            }

            if (tmpSelected != -1 && Slots[tmpSelected].ItemId.Value != Items.Nothing)
            {
                _service.SwapSlots(tmpSelected, currSeleted, this);
                currSeleted = -1;
            }

            SelectedChanged.OnNext(currSeleted);
        });


        Slots.Add(viewModel);
    }

    private void RemoveSlotViewModel(InventorySlot origin)
    {
        throw new NotImplementedException();
    }

    public void RequestSortInventory()
    {
        currSeleted = -1;
        SelectedChanged.OnNext(currSeleted);

        UIManager.RequestSortInventory(OwnerId);
    }

    public void RequestThrow()
    {
        currSeleted = -1;
        SelectedChanged.OnNext(currSeleted);


        return;
    }
}