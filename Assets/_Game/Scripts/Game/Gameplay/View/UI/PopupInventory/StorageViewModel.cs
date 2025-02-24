
using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;

public class StorageViewModel
{
    private PopupInventoryViewModel _parrent;
    public List<InventorySlotViewModel> Slots { get; } = new();

    public Subject<IElementInfoViewModel> CreateElementInfo = new();
    public Subject<IElementInfoViewModel> DeleteElementInfo = new();

    public StorageViewModel(Storage origin, PopupInventoryViewModel parrent)
    {
        _parrent = parrent;

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
            throw new NotImplementedException();
        });
    }

    public void SetParrent(PopupInventoryViewModel parrent)
    {
        _parrent = parrent;
    }


    private void CreateSlotViewModel(InventorySlot origin)
    {
        var viewModel = new InventorySlotViewModel(origin);
        viewModel.SelectRequested.Subscribe(s =>
        {
            var tmpSelected = _parrent.CurrSelectedItem;
            _parrent.CurrSelectedItem = viewModel;
            _parrent.SelectedChanged.OnNext(viewModel);

            if (_parrent.CurrSelectedEquip == null)
                _parrent.SwapItems(tmpSelected);
            else
                _parrent.EquipItem();

        });

        viewModel.Rarity = _parrent.ItemsConfig[viewModel.ItemId.Value].Rarity;
        viewModel.ItemDescription = _parrent.ItemsConfig[viewModel.ItemId.Value].Desription;
        viewModel.OnMouseEnter.Subscribe(e => CreateElementInfo.OnNext(e));
        viewModel.OnMouseExit.Subscribe(e => DeleteElementInfo.OnNext(e));

        Slots.Add(viewModel);
    }
}