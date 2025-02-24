using R3;

public static class GameplayViewModelsRegistrations
{
    public static void Register(DIContainer sceneContainer)
    {
        sceneContainer.RegisterFactory(c => new GameplayUIManager(sceneContainer)).AsSingle();

        var inputRequests = sceneContainer.Resolve<InputRequests>();
        sceneContainer.RegisterFactory(c => new UIGameplayRootViewModel(inputRequests)).AsSingle();

        sceneContainer.RegisterFactory(c => new WorldGameplayRootViewModel(sceneContainer.Resolve<CreaturesSerivce>(), sceneContainer)).AsSingle();
    }
}
