using R3;

public class PopupDevPanelViewModel : WindowViewModel
{
    private readonly InventoriesService _inventoriesService;
    private readonly CreaturesSerivce _creaturesSerivce;

    public override string Id => "PopupDevPanel";

    public DPInventoriesViewModel InventoriesPageViewModel { get; }
    public DPCreaturesViewModel CreaturesPageViewModel { get; }

    public Subject<Unit> PrivilegesRequest { get; } = new();

    public PopupDevPanelViewModel(InventoriesService inventoriesService, CreaturesSerivce creaturesSerivce)
    {
        _inventoriesService = inventoriesService;
        _creaturesSerivce = creaturesSerivce;

        InventoriesPageViewModel = new(_inventoriesService, InputRequests);
        CreaturesPageViewModel = new(_creaturesSerivce, InputRequests, this);

        InputRequests.EscapeRequest = new();
        InputRequests.URequest = new();
        InputRequests.MouseRequest = new();

        InputRequests.EscapeRequest.Subscribe(_ => RequestClose());
        InputRequests.URequest.Subscribe(_ => RequestClose());
    }


}
