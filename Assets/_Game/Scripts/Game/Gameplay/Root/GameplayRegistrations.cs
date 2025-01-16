using R3;

public static class GameplayRegistrations
{

    public static void Register(DIContainer sceneContainer, GameplayEnterParams enterParams)
    {
        var gameStateProvider = sceneContainer.Resolve<IGameStateProvider>();
        var gameState = gameStateProvider.GameState;
        var processor = new CommandProcessor(gameStateProvider);
        var configProvider = sceneContainer.Resolve<IConfigProvider>();
        var gameConfig = configProvider.GameConfig;

        processor.RegisterHandler(new CmdDamageCreatureHandler(gameState));
        processor.RegisterHandler(new CmdCreateInventoryHandler(gameState));
        processor.RegisterHandler(new CmdCreateCreatureHandler(gameState, gameConfig.CreaturesConfig));
        processor.RegisterHandler(new CmdAddItemHandler(processor, gameConfig.ItemsConfig, gameState));
        processor.RegisterHandler(new CmdAddItemInSlotHandler(gameConfig.ItemsConfig, gameState));

        sceneContainer.RegisterFactory(_ => new CreaturesSerivce(gameState.Creatures, gameConfig.CreaturesConfig,processor)).AsSingle();
        sceneContainer.RegisterFactory(_ => new InventoriesService(gameState.Inventories, gameConfig.ItemsConfig, processor)).AsSingle();
        sceneContainer.RegisterInstance<ICommandProcessor>(processor);
        sceneContainer.RegisterInstance(AppConstants.EXIT_SCENE_REQUEST_TAG, new Subject<Unit>());
    }
}
