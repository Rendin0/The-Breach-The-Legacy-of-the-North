using R3;

public static class MainMenuViewModelsRegistrations
{
    public static void Register(DIContainer sceneContainer)
    {
        sceneContainer.RegisterFactory(c => new MainMenuUIManager(sceneContainer)).AsSingle();

        var inputRequests = sceneContainer.Resolve<InputRequests>();
        sceneContainer.RegisterFactory(c => new UIMainMenuRootViewModel(inputRequests)).AsSingle();
    }

}
