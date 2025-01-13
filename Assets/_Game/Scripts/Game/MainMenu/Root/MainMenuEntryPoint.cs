using System;
using UnityEngine;
using R3;

public class MainMenuEntryPoint : MonoBehaviour
{
    [SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;

    public Observable<MainMenuExitParams> Run(DIContainer sceneContainer, MainMenuEnterParams enterParams)
    {
        MainMenuRegistrations.Register(sceneContainer, enterParams);
        var mainMenuViewModelsContainer = new DIContainer(sceneContainer);
        MainMenuViewModelsRegistrations.Register(mainMenuViewModelsContainer);


        var uiRoot = sceneContainer.Resolve<UIRootView>();
        var uiScene = Instantiate(_sceneUIRootPrefab);
        uiRoot.AttachSceneUI(uiScene.gameObject);

        var exitSubject = new Subject<Unit>();
        uiScene.Bind(exitSubject);

        var gameplayEnterParams = new GameplayEnterParams();
        var mainMenuExitParams = new MainMenuExitParams(gameplayEnterParams);

        Debug.Log($"Main menu enter params: {enterParams?.EnterParams}");

        var exitScene = exitSubject.Select(_ => mainMenuExitParams);

        return exitScene;
    }
}
