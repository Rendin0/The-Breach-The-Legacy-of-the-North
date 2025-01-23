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
        //var player = creaturesSerivce.GetPlayer();
        //inventoriesService.CreateInventory(player.CreatureId, 16);
        //inventoriesService.AddSlotsToInventory(player.CreatureId, 16);
        //inventoriesService.AddItemInInventory(player.CreatureId, "Sword", 1);
        //inventoriesService.AddItemInInventory(player.CreatureId, "Sword", 1);
        //inventoriesService.AddItemInInventory(player.CreatureId, "Lava", 3);
        //inventoriesService.AddItemInInventory(player.CreatureId, "CoolLava", 6);
        //inventoriesService.AddItemInInventory(player.CreatureId, "MediumLava", 8);

        //creaturesSerivce.CreateCreature("Skeleton", Vector3.zero);
        //inventoriesService.CreateInventory(1, 16);
        //inventoriesService.AddItemInInventory(1, "CoolLava", 5);

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

        // �������� ����
        var uiManager = viewsContainer.Resolve<GameplayUIManager>();
        uiManager.OpenScreenGameplay();
    }
}
