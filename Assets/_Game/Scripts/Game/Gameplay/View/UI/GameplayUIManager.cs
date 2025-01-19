
using R3;

public class GameplayUIManager : UIManager
{
    private readonly CompositeDisposable _subs = new();

    private readonly Subject<Unit> _exitSceneRequest;


    public GameplayUIManager(DIContainer container) : base(container)
    {
        _exitSceneRequest = container.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
        var escapeRequest = container.Resolve<Subject<Unit>>(AppConstants.ESCAPE_REQUEST_TAG);
        var tabRequest = container.Resolve<Subject<Unit>>(AppConstants.TAB_REQUEST_TAG);


    }

    public ScreenGameplayPauseViewModel OpenScreenGameplayPause()
    {
        var input = Container.Resolve<GameInput>();
        input.Player.Disable();

        var viewModel = new ScreenGameplayPauseViewModel(this, _exitSceneRequest);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenScreen(viewModel);
        input.UI.Enable();

        return viewModel;
    }
    public ScreenGameplayViewModel OpenScreenGameplay()
    {
        var input = Container.Resolve<GameInput>();
        input.UI.Disable();


        var viewModel = new ScreenGameplayViewModel(this);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenScreen(viewModel);
        input.Player.Enable();

        return viewModel;
    }

    public PopupSettingsViewModel OpenPopupSettings(ScreenGameplayPauseViewModel prevWindow)
    {
        var viewModel = new PopupSettingsViewModel();
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenPopup(viewModel, prevWindow);
        return viewModel;
    }
    public PopupInventoryViewModel OpenPopupInventory(int ownerId, ScreenGameplayViewModel prevWindow)
    {
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();
        var inventoryService = Container.Resolve<InventoriesService>();
        var viewModel = inventoryService.GetInventory(ownerId);

        viewModel.UIManager = this;
        rootUI.OpenPopup(viewModel, prevWindow);
        return viewModel;
    }

    public void RequestSortInventory(int ownerId)
    {
        var inventoryService = Container.Resolve<InventoriesService>();
        inventoryService.SortInventory(ownerId);
    }


}