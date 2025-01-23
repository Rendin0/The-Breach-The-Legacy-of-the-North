using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;

public class PopupInventoryViewModel : WindowViewModel
{ 
    public GameplayUIManager UIManager;
    public readonly InventoryGrid Origin;

    public override string Id => "PopupInventory";
    public int OwnerId;

    public readonly StorageViewModel Storage;
    public ReactiveProperty<StorageViewModel> TmpStorage = new(null);
    public Dictionary<EquipmentType, InventorySlotViewModel> Equipment { get; } = new();
    public Subject<InventorySlotViewModel> SelectedChanged = new();

    public InventorySlotViewModel CurrSelectedItem = null;
    public EquipmentType? CurrSelectedEquip = null;
    private readonly InventoriesService _service;
    public PopupInventoryViewModel(InventoryGrid origin, InventoriesService service)
    {
        Origin = origin;
        Storage = new StorageViewModel(origin.Storage);
        Storage.SetParrent(this);
        

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
            throw new NotImplementedException();
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
            var prevSelected = CurrSelectedEquip;
            CurrSelectedEquip = type;

            SelectedChanged.OnNext(viewModel);

            // Игрок дважды нажал на один слот, хочет отменить действие
            if (prevSelected == CurrSelectedEquip)
            {
                CurrSelectedEquip = null;
                SelectedChanged.OnNext(null);
                return;
            }

            if (CurrSelectedItem == null || CurrSelectedItem.ItemId.Value == ItemsIDs.Nothing)
            {
                SelectedChanged.OnNext(viewModel);
                return;
            }

            EquipItem();
        });

        Equipment[type] = viewModel;
    }

    public void SwapItems(InventorySlotViewModel tmpSelected)
    {
        if (CurrSelectedItem == tmpSelected)
        {
            SelectedChanged.OnNext(null);
            tmpSelected = CurrSelectedItem = null;
        }

        if (tmpSelected != null && tmpSelected.ItemId.Value != ItemsIDs.Nothing)
        {
            _service.SwapSlots(tmpSelected, CurrSelectedItem, this);
            SelectedChanged.OnNext(null);
            CurrSelectedItem = null;
        }

    }
    public void EquipItem()
    {
        _service.SwapEquipment(CurrSelectedItem, (EquipmentType)CurrSelectedEquip, this);
        CurrSelectedEquip = null;
        CurrSelectedItem = null;
        SelectedChanged.OnNext(null);
    }

    

    public void RequestSortInventory()
    {
        CurrSelectedItem = null;
        SelectedChanged.OnNext(null);

        UIManager.RequestSortInventory(OwnerId);
    }

    public void RequestThrow()
    {
        CurrSelectedItem = null;
        SelectedChanged.OnNext(null);


        return;
    }

    public void FastUnequipRequest()
    {
        _service.FastUnequip(this);
    }
}