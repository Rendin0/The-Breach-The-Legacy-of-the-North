using R3;

public static class GameplayRegistrations
{

    public static void Register(DIContainer sceneContainer, GameplayEnterParams enterParams)
    {
        var gameStateProvider = sceneContainer.Resolve<IGameStateProvider>();
        var gameState = gameStateProvider.GameState;
        var processor = new CommandProcessor(gameStateProvider);
        processor.RegisterHandler(new CmdDamageCreatureHandler(gameState));
        processor.RegisterHandler(new CmdCreateInventoryHandler(gameState));

        var configProvider = sceneContainer.Resolve<IConfigProvider>();
        var gameConfig = configProvider.GameConfig;
        processor.RegisterHandler(new CmdCreateCreatureHandler(gameState, gameConfig.CreaturesConfig));

        sceneContainer.RegisterFactory(_ => new CreaturesSerivce(gameState.Creatures, gameConfig.CreaturesConfig,processor)).AsSingle();
        sceneContainer.RegisterInstance<ICommandProcessor>(processor);
        sceneContainer.RegisterInstance(AppConstants.EXIT_SCENE_REQUEST_TAG, new Subject<Unit>());
    }
}
