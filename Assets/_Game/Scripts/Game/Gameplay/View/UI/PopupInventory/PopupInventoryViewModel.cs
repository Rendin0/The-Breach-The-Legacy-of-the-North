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
    public readonly Dictionary<string, ItemConfig> ItemsConfig;
    public CreatureViewModel Owner;


    public Subject<IElementInfoViewModel> CreateElementInfo = new();
    public Subject<IElementInfoViewModel> DeleteElementInfo = new();

    public PopupInventoryViewModel(InventoryGrid origin, InventoriesService service)
    {
        ItemsConfig = service.ItemsConfig;
        Origin = origin;
        Storage = new StorageViewModel(origin.Storage, this);

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

        InputRequests.EscapeRequest = new();
        InputRequests.TabRequest = new();

        InputRequests.EscapeRequest.Subscribe(_ => Close(_));
        InputRequests.TabRequest.Subscribe(_ => Close(_));

        Storage.CreateElementInfo.Subscribe(e => CreateElementInfo.OnNext(e));
        Storage.DeleteElementInfo.Subscribe(e => DeleteElementInfo.OnNext(e));
    }

    private void Close(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            RequestClose();
    }

    private void CreateEquipmentViewModel(InventorySlot origin, EquipmentType type)
    {
        var viewModel = new InventorySlotViewModel(origin);

        viewModel.SelectRequested.Subscribe(s =>
        {
            var prevSelected = CurrSelectedEquip;
            CurrSelectedEquip = type;

            SelectedChanged.OnNext(viewModel);

            // ����� ������ ����� �� ���� ����, ����� �������� ��������
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
            CurrSelectedItem.ResetColor.OnNext(Unit.Default);
            SelectedChanged.OnNext(null);
            tmpSelected = CurrSelectedItem = null;
        }

        if (tmpSelected != null && tmpSelected.ItemId.Value != ItemsIDs.Nothing)
        {
            CurrSelectedItem.ResetColor.OnNext(Unit.Default);
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



    // ��� �������������� � ��������� ����� ��� ������
    // ���������� ����� ������� �������� � �������� ������
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