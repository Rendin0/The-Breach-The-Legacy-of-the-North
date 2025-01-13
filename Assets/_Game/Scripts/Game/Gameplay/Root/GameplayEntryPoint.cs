using R3;
using System;
using UnityEngine;

public class GameplayEntryPoint : MonoBehaviour
{
    [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;

    public Observable<GameplayExitParams> Run(DIContainer sceneContainer, GameplayEnterParams enterParams)
    {
        GameplayRegistrations.Register(sceneContainer, enterParams);
        var gameplayViewModelsContainer = new DIContainer(sceneContainer);
        GameplayViewModelsRegistrations.Register(gameplayViewModelsContainer);


        var uiRoot = sceneContainer.Resolve<UIRootView>();
        var uiScene = Instantiate(_sceneUIRootPrefab);
        uiRoot.AttachSceneUI(uiScene.gameObject);

        var exitSceneSignalSubject = new Subject<Unit>();

        uiScene.Bind(exitSceneSignalSubject);
         
        var mainMenuEnterParams = new MainMenuEnterParams("Lul");
        var exitParams = new GameplayExitParams(mainMenuEnterParams);

        var exitToMainMenuSceneSignal = exitSceneSignalSubject.Select(_ => exitParams);

        return exitToMainMenuSceneSignal;
    }
}
