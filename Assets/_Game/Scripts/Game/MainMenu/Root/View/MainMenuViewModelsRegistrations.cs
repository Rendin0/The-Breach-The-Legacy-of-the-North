public static class MainMenuViewModelsRegistrations
{
    public static void Register(DIContainer sceneContainer)
    {
        sceneContainer.RegisterFactory(c => new MainMenuUIManager(sceneContainer)).AsSingle();

        sceneContainer.RegisterFactory(c => new UIMainMenuRootViewModel()).AsSingle();
    }

}
