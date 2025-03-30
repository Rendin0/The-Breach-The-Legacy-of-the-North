
using R3;
using UnityEngine;

/// <summary>
/// ����� ��� ���������� UI � ������� ��������.
/// </summary>
public class GameplayUIManager : UIManager
{
    private readonly Subject<Unit> _exitSceneRequest;
    private bool _devPrivileges = false;

    /// <summary>
    /// ����������� ��� ������������� ���������� ������������.
    /// </summary>
    /// <param name="container">��������� ������������.</param>
    public GameplayUIManager(DIContainer container) : base(container)
    {
        _exitSceneRequest = container.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
    }

    #region Developer Privileges
    /// <summary>
    /// ������������ ���������� ������������.
    /// </summary>
    public void ToggleDevPriveleges()
    {
        _devPrivileges = !_devPrivileges;
    }
    #endregion

    #region Screen Management
    /// <summary>
    /// �������� ������ ����� �������� ��������.
    /// </summary>
    /// <returns>������ ������������� ������ �����.</returns>
    public ScreenGameplayPauseViewModel OpenScreenGameplayPause()
    {
        // ����� ����
        Time.timeScale = 0.0f;

        var viewModel = new ScreenGameplayPauseViewModel(this, _exitSceneRequest);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenScreen(viewModel);

        return viewModel;
    }

    /// <summary>
    /// �������� ������ �������� ��������.
    /// </summary>
    /// <returns>������ ������������� ������ �������� ��������.</returns>
    public ScreenGameplayViewModel OpenScreenGameplay()
    {
        // ������������� ����
        Time.timeScale = 1.0f;

        var input = Container.Resolve<GameInput>();
        input.UI.Disable();

        var player = Container.Resolve<CreaturesSerivce>().GetPlayer();

        var viewModel = new ScreenGameplayViewModel(this, input.FindAction("Abilities"), player);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenScreen(viewModel);
        input.Player.Enable();

        SubscribeElementInfo(viewModel.CreateElementInfo, viewModel.DeleteElementInfo);

        return viewModel;
    }
    #endregion

    #region Popup Management
    /// <summary>
    /// �������� ������������ ���� � ����������� �� ��������.
    /// </summary>
    /// <param name="elementInfo">���������� �� ��������.</param>
    /// <returns>������ ������������� ������������ ����.</returns>
    public PopupElementInfoViewModel OpenPopupElementInfo(IElementInfoViewModel elementInfo)
    {
        var viewModel = new PopupElementInfoViewModel(elementInfo);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenPopup(viewModel);

        return viewModel;
    }

    /// <summary>
    /// �������� ������������ ���� � ����������� �� ��������.
    /// </summary>
    /// <param name="elementInfo">������ ������������� ����.</param>
    public void ClosePopupElementInfo(WindowViewModel elementInfo)
    {
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.ClosePopup(elementInfo);
    }

    /// <summary>
    /// �������� ������������ ���� ����� ����.
    /// </summary>
    /// <param name="player">������ ������������� ������.</param>
    /// <returns>������ ������������� ������������ ���� ����� ����.</returns>
    public PopupWorldMapViewModel OpenPopupWorldMap(PlayerViewModel player)
    {
        var viewModel = new PopupWorldMapViewModel(player);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenPopup(viewModel);

        return viewModel;
    }

    /// <summary>
    /// �������� ������������ ���� ��������.
    /// </summary>
    /// <returns>������ ������������� ������������ ���� ��������.</returns>
    public PopupSettingsViewModel OpenPopupSettings()
    {
        var viewModel = new PopupSettingsViewModel();
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenPopup(viewModel);
        return viewModel;
    }

    /// <summary>
    /// �������� ������������ ���� ���� ��������.
    /// </summary>
    /// <param name="creatureViewModel">������ ������������� ��������.</param>
    /// <returns>������ ������������� ������������ ���� ���� ��������.</returns>
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

    /// <summary>
    /// �������� ������������ ���� ���������� � ��������.
    /// </summary>
    /// <param name="creatureViewModel">������ ������������� ��������.</param>
    /// <returns>������ ������������� ������������ ���� ���������� � ��������.</returns>
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

    /// <summary>
    /// �������� ������������ ���� ���������.
    /// </summary>
    /// <param name="ownerId">������������� ��������� ���������.</param>
    /// <returns>������ ������������� ������������ ���� ���������.</returns>
    public PopupInventoryViewModel OpenPopupInventory(int ownerId)
    {
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();
        var inventoryService = Container.Resolve<InventoriesService>();
        var creatureService = Container.Resolve<CreaturesSerivce>();
        var inventory = inventoryService.GetInventory(ownerId);
        inventory.Owner = creatureService.GetPlayer();

        inventory.UIManager = this;
        rootUI.OpenPopup(inventory);

        SubscribeElementInfo(inventory.CreateElementInfo, inventory.DeleteElementInfo);

        return inventory;
    }

    /// <summary>
    /// �������� ������������ ���� �������.
    /// </summary>
    /// <returns>������ ������������� ������������ ���� �������.</returns>
    public PopupDialogueViewModel OpenPopupDialog()
    {
        return null;
    }

    /// <summary>
    /// �������� ������������ ���� ������ ������������.
    /// </summary>
    /// <returns>������ ������������� ������������ ���� ������ ������������.</returns>
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

    /// <summary>
    /// �������� ���������.
    /// </summary>
    /// <param name="storageId">������������� ���������.</param>
    /// <param name="parent">������������ ������ ������������� ������������ ���� ���������.</param>
    /// <returns>������ ������������� ���������.</returns>
    public StorageViewModel OpenStorage(int storageId, PopupInventoryViewModel parent)
    {
        var inventoryService = Container.Resolve<InventoriesService>();
        var storage = inventoryService.GetInventory(storageId).Storage;
        storage.SetParent(parent);
        parent.TmpStorage.OnNext(storage);

        return storage;
    }
    #endregion

    #region Inventory Management
    /// <summary>
    /// ������ �� ���������� ���������.
    /// </summary>
    /// <param name="ownerId">������������� ��������� ���������.</param>
    public void RequestSortInventory(int ownerId)
    {
        var inventoryService = Container.Resolve<InventoriesService>();
        inventoryService.SortInventory(ownerId);
    }
    #endregion

    #region Element Info Subscription
    /// <summary>
    /// �������� �� ������� ���������� �� ��������.
    /// </summary>
    /// <param name="onEnter">������� ��� ��������� �� �������.</param>
    /// <param name="onExit">������� ��� ����� � ��������.</param>
    private void SubscribeElementInfo(Subject<IElementInfoViewModel> onEnter, Subject<IElementInfoViewModel> onExit)
    {
        WindowViewModel viewModel = null;

        onEnter.Subscribe(e => viewModel = OpenPopupElementInfo(e));
        onExit.Subscribe(e => ClosePopupElementInfo(viewModel));
    }
    #endregion
}
