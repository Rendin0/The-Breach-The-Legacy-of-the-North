
using ObservableCollections;
using System.Collections.Generic;

public class PopupInventoryViewModel : WindowViewModel
{
    public override string Id => "PopupInventory";

    public List<InventorySlotViewModel> Slots { get; }

    public PopupInventoryViewModel()
    {
        
    }
}