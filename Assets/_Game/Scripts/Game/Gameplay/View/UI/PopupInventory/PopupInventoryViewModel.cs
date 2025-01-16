using R3;
using ObservableCollections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PopupInventoryViewModel : WindowViewModel
{
    public GameplayUIManager UIManager;

    public override string Id => "PopupInventory";
    public int OwnerId;

    public List<InventorySlotViewModel> Slots { get; } = new();

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

        OwnerId = origin.OwnerId;
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

    public void RequestSortInventory()
    {
        UIManager.RequestSortInventory(OwnerId);
    }
}