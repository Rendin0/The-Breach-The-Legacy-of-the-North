using R3;
using ObservableCollections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PopupInventoryViewModel : WindowViewModel
{
    public override string Id => "PopupInventory";

    public List<InventorySlotViewModel> Slots { get; }

    public PopupInventoryViewModel(InventoryGrid origin)
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
    }

    private void CreateSlotViewModel(InventorySlot origin)
    {
        var viewModel = new InventorySlotViewModel(origin);

        Slots.Add(viewModel);
    }

    private void RemoveSlotViewModel(InventorySlot origin)
    {
        throw new NotImplementedException();
    }
}