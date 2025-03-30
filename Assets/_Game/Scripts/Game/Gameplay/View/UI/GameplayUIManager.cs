
using R3;
using UnityEngine;

/// <summary>
/// Класс для управления UI в игровом процессе.
/// </summary>
public class GameplayUIManager : UIManager
{
    private readonly Subject<Unit> _exitSceneRequest;
    private bool _devPrivileges = false;

    /// <summary>
    /// Конструктор для инициализации контейнера зависимостей.
    /// </summary>
    /// <param name="container">Контейнер зависимостей.</param>
    public GameplayUIManager(DIContainer container) : base(container)
    {
        _exitSceneRequest = container.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
    }

    #region Developer Privileges
    /// <summary>
    /// Переключение привилегий разработчика.
    /// </summary>
    public void ToggleDevPriveleges()
    {
        _devPrivileges = !_devPrivileges;
    }
    #endregion

    #region Screen Management
    /// <summary>
    /// Открытие экрана паузы игрового процесса.
    /// </summary>
    /// <returns>Модель представления экрана паузы.</returns>
    public ScreenGameplayPauseViewModel OpenScreenGameplayPause()
    {
        // Пауза игры
        Time.timeScale = 0.0f;

        var viewModel = new ScreenGameplayPauseViewModel(this, _exitSceneRequest);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenScreen(viewModel);

        return viewModel;
    }

    /// <summary>
    /// Открытие экрана игрового процесса.
    /// </summary>
    /// <returns>Модель представления экрана игрового процесса.</returns>
    public ScreenGameplayViewModel OpenScreenGameplay()
    {
        // Возобновление игры
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
    /// Открытие всплывающего окна с информацией об элементе.
    /// </summary>
    /// <param name="elementInfo">Информация об элементе.</param>
    /// <returns>Модель представления всплывающего окна.</returns>
    public PopupElementInfoViewModel OpenPopupElementInfo(IElementInfoViewModel elementInfo)
    {
        var viewModel = new PopupElementInfoViewModel(elementInfo);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenPopup(viewModel);

        return viewModel;
    }

    /// <summary>
    /// Закрытие всплывающего окна с информацией об элементе.
    /// </summary>
    /// <param name="elementInfo">Модель представления окна.</param>
    public void ClosePopupElementInfo(WindowViewModel elementInfo)
    {
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.ClosePopup(elementInfo);
    }

    /// <summary>
    /// Открытие всплывающего окна карты мира.
    /// </summary>
    /// <param name="player">Модель представления игрока.</param>
    /// <returns>Модель представления всплывающего окна карты мира.</returns>
    public PopupWorldMapViewModel OpenPopupWorldMap(PlayerViewModel player)
    {
        var viewModel = new PopupWorldMapViewModel(player);
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenPopup(viewModel);

        return viewModel;
    }

    /// <summary>
    /// Открытие всплывающего окна настроек.
    /// </summary>
    /// <returns>Модель представления всплывающего окна настроек.</returns>
    public PopupSettingsViewModel OpenPopupSettings()
    {
        var viewModel = new PopupSettingsViewModel();
        var rootUI = Container.Resolve<UIGameplayRootViewModel>();

        rootUI.OpenPopup(viewModel);
        return viewModel;
    }

    /// <summary>
    /// Открытие всплывающего окна меню существа.
    /// </summary>
    /// <param name="creatureViewModel">Модель представления существа.</param>
    /// <returns>Модель представления всплывающего окна меню существа.</returns>
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
    /// Открытие всплывающего окна информации о существе.
    /// </summary>
    /// <param name="creatureViewModel">Модель представления существа.</param>
    /// <returns>Модель представления всплывающего окна информации о существе.</returns>
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
    /// Открытие всплывающего окна инвентаря.
    /// </summary>
    /// <param name="ownerId">Идентификатор владельца инвентаря.</param>
    /// <returns>Модель представления всплывающего окна инвентаря.</returns>
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
    /// Открытие всплывающего окна диалога.
    /// </summary>
    /// <returns>Модель представления всплывающего окна диалога.</returns>
    public PopupDialogueViewModel OpenPopupDialog()
    {
        return null;
    }

    /// <summary>
    /// Открытие всплывающего окна панели разработчика.
    /// </summary>
    /// <returns>Модель представления всплывающего окна панели разработчика.</returns>
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
    /// Открытие хранилища.
    /// </summary>
    /// <param name="storageId">Идентификатор хранилища.</param>
    /// <param name="parent">Родительская модель представления всплывающего окна инвентаря.</param>
    /// <returns>Модель представления хранилища.</returns>
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
    /// Запрос на сортировку инвентаря.
    /// </summary>
    /// <param name="ownerId">Идентификатор владельца инвентаря.</param>
    public void RequestSortInventory(int ownerId)
    {
        var inventoryService = Container.Resolve<InventoriesService>();
        inventoryService.SortInventory(ownerId);
    }
    #endregion

    #region Element Info Subscription
    /// <summary>
    /// Подписка на события информации об элементе.
    /// </summary>
    /// <param name="onEnter">Событие при наведении на элемент.</param>
    /// <param name="onExit">Событие при уходе с элемента.</param>
    private void SubscribeElementInfo(Subject<IElementInfoViewModel> onEnter, Subject<IElementInfoViewModel> onExit)
    {
        WindowViewModel viewModel = null;

        onEnter.Subscribe(e => viewModel = OpenPopupElementInfo(e));
        onExit.Subscribe(e => ClosePopupElementInfo(viewModel));
    }
    #endregion
}
