using R3;
using UnityEngine;

public class GameplayEntryPoint : MonoBehaviour
{
    [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
    [SerializeField] private WorldGameplayRootBinder _worldBinder;

    public Observable<GameplayExitParams> Run(DIContainer sceneContainer, GameplayEnterParams enterParams)
    {
        GameplayRegistrations.Register(sceneContainer, enterParams);
        var gameplayViewModelsContainer = new DIContainer(sceneContainer);
        GameplayViewModelsRegistrations.Register(gameplayViewModelsContainer);

        InitUI(gameplayViewModelsContainer);
        _worldBinder.Bind(gameplayViewModelsContainer.Resolve<WorldGameplayRootViewModel>());

        var creaturesSerivce = gameplayViewModelsContainer.Resolve<CreaturesSerivce>();

        var inventoriesService = gameplayViewModelsContainer.Resolve<InventoriesService>();
        var player = creaturesSerivce.GetPlayer();
        //inventoriesService.CreateInventory(player.CreatureId, 32);
        //inventoriesService.AddItemInInventory(player.CreatureId, "Weapon", 1);
        //inventoriesService.AddItemInInventory(player.CreatureId, "Chest", 1);
        //inventoriesService.AddItemInInventory(player.CreatureId, "Shoulder", 1);
        //inventoriesService.AddItemInInventory(player.CreatureId, "Belt", 1);
        //inventoriesService.AddItemInInventory(player.CreatureId, "Pants", 1);
        //inventoriesService.AddItemInInventory(player.CreatureId, "Helmet", 1);
        //inventoriesService.AddItemInInventory(player.CreatureId, "Boots", 1);
        //inventoriesService.AddItemInInventory(player.CreatureId, "Rune", 1);

        //creaturesSerivce.CreateCreature("Skeleton", Vector3.zero);
        //inventoriesService.CreateInventory(1, 16);
        //inventoriesService.AddItemInInventory(1, "Sword", 1);
        //inventoriesService.AddItemInInventory(1, "Lava", 1);
        //inventoriesService.AddItemInInventory(1, "MediumLava", 1);
        //inventoriesService.AddItemInInventory(1, "CoolLava", 1);

        var mainMenuEnterParams = new MainMenuEnterParams("Lul");
        var exitParams = new GameplayExitParams(mainMenuEnterParams);

        var exitSceneRequest = sceneContainer.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
        var exitToMainMenuSceneSignal = exitSceneRequest.Select(_ => exitParams);

        return exitToMainMenuSceneSignal;
    }

    private void InitUI(DIContainer viewsContainer)
    {
        var uiRoot = viewsContainer.Resolve<UIRootView>();
        var uiScene = Instantiate(_sceneUIRootPrefab);
        uiRoot.AttachSceneUI(uiScene.gameObject);

        var uiSceneRootViewModel = viewsContainer.Resolve<UIGameplayRootViewModel>();
        uiScene.Bind(uiSceneRootViewModel);

        // открытие окон
        var uiManager = viewsContainer.Resolve<GameplayUIManager>();
        uiManager.OpenScreenGameplay();
    }
}
