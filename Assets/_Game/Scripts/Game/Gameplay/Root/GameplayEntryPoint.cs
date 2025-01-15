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
        creaturesSerivce.CreateCreature("Skeleton", Vector3.zero);

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
