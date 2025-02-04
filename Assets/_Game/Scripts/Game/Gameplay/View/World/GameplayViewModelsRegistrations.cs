using R3;

public static class GameplayViewModelsRegistrations
{
    public static void Register(DIContainer sceneContainer)
    {
        sceneContainer.RegisterFactory(c => new GameplayUIManager(sceneContainer)).AsSingle();

        var inputRequests = new InputRequests
        {
            EscapeRequest = sceneContainer.Resolve<Subject<Unit>>(AppConstants.ESCAPE_REQUEST_TAG),
            TabRequest = sceneContainer.Resolve<Subject<Unit>>(AppConstants.TAB_REQUEST_TAG),
            URequest = sceneContainer.Resolve<Subject<Unit>>(AppConstants.U_REQUEST_TAG),
            MouseRequest = sceneContainer.Resolve<Subject<Unit>>(AppConstants.MOUSE_REQUEST_TAG)
        };
        sceneContainer.RegisterFactory(c => new UIGameplayRootViewModel(inputRequests)).AsSingle();

        sceneContainer.RegisterFactory(c => new WorldGameplayRootViewModel(sceneContainer.Resolve<CreaturesSerivce>(), sceneContainer)).AsSingle();
    }
}
