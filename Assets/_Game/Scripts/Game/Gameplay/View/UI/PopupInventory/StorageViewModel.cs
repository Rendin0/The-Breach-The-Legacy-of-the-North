
using System.Collections.Generic;
using R3;
using ObservableCollections;
using System;

public class StorageViewModel
{
    private PopupInventoryViewModel _parrent;
    public List<InventorySlotViewModel> Slots { get; } = new();

    public StorageViewModel(Storage origin)
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


        Slots.Add(viewModel);
    }
}