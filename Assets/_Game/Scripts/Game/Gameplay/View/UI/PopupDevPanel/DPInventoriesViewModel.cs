
public class DPInventoriesViewModel
{
    private readonly InventoriesService _inventoriesService;
    private readonly InputRequests _inputRequests;

    public DPInventoriesViewModel(InventoriesService inventoriesService, InputRequests inputRequests)
    {
        this._inventoriesService = inventoriesService;
        _inputRequests = inputRequests;
    }

    public void AddInventory(int ownerId, int slotsAmount)
    {
        _inventoriesService.CreateInventory(ownerId, slotsAmount);
    }

    public void AddSlots(int ownerId, int slotsAmount)
    {
        _inventoriesService.AddSlotsToInventory(ownerId, slotsAmount);
    }

}