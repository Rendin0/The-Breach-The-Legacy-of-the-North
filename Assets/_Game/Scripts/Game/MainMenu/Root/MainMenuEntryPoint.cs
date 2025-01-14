using System;
using UnityEngine;
using R3;
using static UnityEngine.ParticleSystem;

public class MainMenuEntryPoint : MonoBehaviour
{
    [SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;

    public Observable<MainMenuExitParams> Run(DIContainer sceneContainer, MainMenuEnterParams enterParams)
    {
        MainMenuRegistrations.Register(sceneContainer, enterParams);
        var mainMenuViewModelsContainer = new DIContainer(sceneContainer);
        MainMenuViewModelsRegistrations.Register(mainMenuViewModelsContainer);

        InitUI(mainMenuViewModelsContainer);

        var gameplayEnterParams = new GameplayEnterParams();
        var mainMenuExitParams = new MainMenuExitParams(gameplayEnterParams);

        var exitSceneRequest = sceneContainer.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
        var exitToMainMenuSceneSignal = exitSceneRequest.Select(_ => mainMenuExitParams);

        return exitToMainMenuSceneSignal;
    }

    private void InitUI(DIContainer viewsContainer)
    {
        var uiRoot = viewsContainer.Resolve<UIRootView>();
        var uiScene = Instantiate(_sceneUIRootPrefab);
        uiRoot.AttachSceneUI(uiScene.gameObject);

        var uiSceneRootViewModel = viewsContainer.Resolve<UIMainMenuRootViewModel>();
        uiScene.Bind(uiSceneRootViewModel);

        // открытие окон
        var uiManager = viewsContainer.Resolve<MainMenuUIManager>();
        uiManager.OpenMainMenu();
    }
}
