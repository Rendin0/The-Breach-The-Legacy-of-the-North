using R3;

public static class GameplayViewModelsRegistrations
{
    public static void Register(DIContainer sceneContainer)
    {
        sceneContainer.RegisterFactory(c => new GameplayUIManager(sceneContainer)).AsSingle();

        var escapeSignal = sceneContainer.Resolve<Subject<Unit>>(AppConstants.ESCAPE_REQUEST_TAG);
        var tabSignal = sceneContainer.Resolve<Subject<Unit>>(AppConstants.TAB_REQUEST_TAG);
        sceneContainer.RegisterFactory(c => new UIGameplayRootViewModel(escapeSignal, tabSignal)).AsSingle();

        sceneContainer.RegisterFactory(c => new WorldGameplayRootViewModel(sceneContainer.Resolve<CreaturesSerivce>())).AsSingle();
    }
}
