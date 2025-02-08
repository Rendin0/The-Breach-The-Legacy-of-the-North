
using R3;
using UnityEngine;

public class GameplayUIManager : UIManager
{
    private readonly Subject<Unit> _exitSceneRequest;

    private bool _devPrivileges = false;

    public void ToggleDevPriveleges()
    {
        _devPrivileges = !_devPrivileges;
    }

    public GameplayUIManager(DIContainer container) : base(container)
    {
        _exitSceneRequest = container.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
    }
    public ScreenGameplayPauseViewModel OpenScreenGameplayPause()
    {
        // Пауза игры
        Time.timeScale = 0.0f;

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
        // Возобновление игры
        Time.timeScale = 1.0f;

        var input = Container.Resolve<GameInput>();
        input.UI.Disable();

        var abilitiesConfig = Container.Resolve<IConfigProvider>().GameConfig.AbilitiesConfig;

        var viewModel = new ScreenGameplayViewModel(this, abilitiesConfig);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenScreen(viewModel);
        input.Player.Enable();

        return viewModel;
    }

    public PopupSettingsViewModel OpenPopupSettings()
    {
        var viewModel = new PopupSettingsViewModel();
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenPopup(viewModel);
        return viewModel;
    }
    public PopupCreatureMenuViewModel OpenPopupCreatureMenu(CreatureViewModel creatureViewModel)
    {
        if (_devPrivileges)
        {
            var viewModel = new PopupCreatureMenuViewModel(creatureViewModel, Input.mousePosition, this);
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenPopup(viewModel);
            return viewModel;
        }

        return null;
    }

    public PopupCreatureInfoViewModel OpenPopupCreatureInfo(CreatureViewModel creatureViewModel)
    {
        if (_devPrivileges)
        {
            var inventoryService = Container.Resolve<InventoriesService>();
            var viewModel = new PopupCreatureInfoViewModel(creatureViewModel, inventoryService);
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenPopup(viewModel);

            return viewModel;
        }

        return null;
    }

    public PopupInventoryViewModel OpenPopupInventory(int ownerId)
    {
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();
        var inventoryService = Container.Resolve<InventoriesService>();
        var inventory = inventoryService.GetInventory(ownerId);

        inventory.UIManager = this;
        rootUI.OpenPopup(inventory);

        return inventory;
    }
    public PopupDialogueViewModel OpenPopupDialog()
    {

        return null;
    }

    public PopupDevPanelViewModel OpenPopupDevPanel()
    {
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();
        var inventoryService = Container.Resolve<InventoriesService>();
        var creatureService = Container.Resolve<CreaturesSerivce>();

        var viewModel = new PopupDevPanelViewModel(inventoryService, creatureService);
        viewModel.PrivilegesRequest.Subscribe(_ => ToggleDevPriveleges());
        rootUI.OpenPopup(viewModel);

        return viewModel;
    }

    public StorageViewModel OpenStorage(int storageId, PopupInventoryViewModel parrent)
    {
        var inventoryService = Container.Resolve<InventoriesService>();
        var storage = inventoryService.GetInventory(storageId).Storage;
        storage.SetParrent(parrent);
        parrent.TmpStorage.OnNext(storage);

        return storage;
    }

    public void RequestSortInventory(int ownerId)
    {
        var inventoryService = Container.Resolve<InventoriesService>();
        inventoryService.SortInventory(ownerId);
    }


}