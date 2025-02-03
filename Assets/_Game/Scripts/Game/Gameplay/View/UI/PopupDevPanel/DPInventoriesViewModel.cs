
public class DPInventoriesViewModel
{
    private readonly InventoriesService _inventoriesService;
    private readonly InputRequests _inputRequests;

    public DPInventoriesViewModel(InventoriesService inventoriesService, InputRequests inputRequests)
    {
        this._inventoriesService = inventoriesService;
        _inputRequests = inputRequests;
    }

}