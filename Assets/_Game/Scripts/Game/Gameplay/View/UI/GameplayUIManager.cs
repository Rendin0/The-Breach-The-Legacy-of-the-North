
using R3;

public class GameplayUIManager : UIManager
{
    private readonly Subject<Unit> _exitSceneRequest;

    public GameplayUIManager(DIContainer container) : base(container)
    {
        _exitSceneRequest = container.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
    }

    public ScreenGameplayPauseViewModel OpenScreenGameplayPause()
    {
        var input = Container.Resolve<GameInput>();
        input.UI.Enable();
        input.Player.Disable();

        var viewModel = new ScreenGameplayPauseViewModel(this, _exitSceneRequest);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenScreen(viewModel);

        return viewModel;
    }

    public ScreenGameplayViewModel OpenScreenGameplay()
    {
        var input = Container.Resolve<GameInput>();
        input.UI.Disable();
        input.Player.Enable();

        var viewModel = new ScreenGameplayViewModel(this);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenScreen(viewModel);

        return viewModel;
    }

    public PopupSettingsViewModel OpenPopupSettings()
    {
        var viewModel = new PopupSettingsViewModel();
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenPopup(viewModel);
        return viewModel;
    }


    public void RequestSortInventory(int ownerId)
    {
        var inventoryService = Container.Resolve<InventoriesService>();
        inventoryService.SortInventory(ownerId);
    }

    public PopupInventoryViewModel OpenPopupInventory(int ownerId)
    {
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();
        var inventoryService = Container.Resolve<InventoriesService>();
        var viewModel = inventoryService.GetInventory(ownerId);
        viewModel.UIManager = this;
        rootUI.OpenPopup(viewModel);
        return viewModel;
    }

}