public static class MainMenuViewModelsRegistrations
{
    public static void Register(DIContainer sceneContainer)
    {
        sceneContainer.RegisterFactory(c => new MainMenuUIManager(sceneContainer)).AsSingle();

        var inputRequests = sceneContainer.Resolve<InputRequests>(AppConstants.MAINMENU_REQUESTS);
        sceneContainer.RegisterFactory(c => new UIMainMenuRootViewModel(inputRequests)).AsSingle();
    }

}
