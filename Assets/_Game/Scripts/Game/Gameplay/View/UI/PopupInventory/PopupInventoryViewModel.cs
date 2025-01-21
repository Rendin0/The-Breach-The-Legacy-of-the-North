using R3;
using ObservableCollections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using JetBrains.Annotations;

public class PopupInventoryViewModel : WindowViewModel
{
    public GameplayUIManager UIManager;
    public readonly InventoryGrid Origin;

    public override string Id => "PopupInventory";
    public int OwnerId;

    public Dictionary<EquipmentType, InventorySlotViewModel> Equipment { get; } = new();
    public List<InventorySlotViewModel> Slots { get; } = new();
    public Subject<InventorySlotViewModel> SelectedChanged = new();

    private int currSelectedItem = -1;
    private EquipmentType? currSelectedEquip = null;
    private readonly InventoriesService _service;

    public PopupInventoryViewModel(InventoryGrid origin, InventoriesService service)
    {
        Origin = origin;

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

        foreach (var equip in origin.Equipment)
        {
            CreateEquipmentViewModel(equip.Value, equip.Key);
        }
        origin.Equipment.ObserveAdd().Subscribe(e =>
        {
            CreateEquipmentViewModel(e.Value.Value, e.Value.Key);
        });
        origin.Equipment.ObserveRemove().Subscribe(e =>
        {
            RemoveSlotViewModel(e.Value.Value);
        });

        OwnerId = origin.OwnerId;
        _service = service;

        EscapeRequest.Subscribe(_ => RequestClose());
        TabRequest.Subscribe(_ => RequestClose());
    }

    private void CreateEquipmentViewModel(InventorySlot origin, EquipmentType type)
    {
        var viewModel = new InventorySlotViewModel(origin);

        viewModel.SelectRequested.Subscribe(s =>
        {
            var prevSelected = currSelectedEquip;
            currSelectedEquip = type;

            SelectedChanged.OnNext(viewModel);

            // Игрок дважды нажал на один слот, хочет отменить действие
            if (prevSelected == currSelectedEquip)
            {
                currSelectedEquip = null;
                SelectedChanged.OnNext(null);
                return;
            }

            if (currSelectedItem == -1 || Slots[currSelectedItem].ItemId.Value == ItemsIDs.Nothing)
            {
                SelectedChanged.OnNext(viewModel);
                return;
            }

            EquipItem();
        });

        Equipment[type] = viewModel;
    }

    private void CreateSlotViewModel(InventorySlot origin)
    {
        var viewModel = new InventorySlotViewModel(origin);
        int index = Slots.Count;
        viewModel.SelectRequested.Subscribe(s =>
        {
            int tmpSelected = currSelectedItem;
            currSelectedItem = index;
            SelectedChanged.OnNext(viewModel);

            if (currSelectedEquip == null)
                SwapItems(tmpSelected);
            else
                EquipItem();

        });


        Slots.Add(viewModel);
    }

    private void SwapItems(int tmpSelected)
    {
        if (currSelectedItem == tmpSelected)
        {
            SelectedChanged.OnNext(null);
            tmpSelected = currSelectedItem = -1;
        }

        if (tmpSelected != -1 && Slots[tmpSelected].ItemId.Value != ItemsIDs.Nothing)
        {
            _service.SwapSlots(tmpSelected, currSelectedItem, this);
            SelectedChanged.OnNext(null);
            currSelectedItem = -1;
        }

    }
    private void EquipItem()
    {
        _service.SwapEquipment(currSelectedItem, (EquipmentType)currSelectedEquip, this);
        currSelectedEquip = null;
        currSelectedItem = -1;
        SelectedChanged.OnNext(null);
    }

    private void RemoveSlotViewModel(InventorySlot origin)
    {
        throw new NotImplementedException();
    }

    public void RequestSortInventory()
    {
        currSelectedItem = -1;
        SelectedChanged.OnNext(null);

        UIManager.RequestSortInventory(OwnerId);
    }

    public void RequestThrow()
    {
        currSelectedItem = -1;
        SelectedChanged.OnNext(null);


        return;
    }
}