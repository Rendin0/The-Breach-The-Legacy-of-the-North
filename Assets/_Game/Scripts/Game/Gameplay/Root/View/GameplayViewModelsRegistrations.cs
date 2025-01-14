public static class GameplayViewModelsRegistrations
{
    public static void Register(DIContainer sceneContainer)
    {
        sceneContainer.RegisterFactory(c => new GameplayUIManager(sceneContainer)).AsSingle();

        sceneContainer.RegisterFactory(c => new UIGameplayRootViewModel()).AsSingle();
    }
}
