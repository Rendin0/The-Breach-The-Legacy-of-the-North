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
        processor.RegisterHandler(new CmdCreateCreatureHandler(gameState, gameConfig.CreaturesConfig));
        var creaturesService = new CreaturesSerivce(gameState.Creatures, gameConfig.CreaturesConfig, gameConfig.AbilitiesConfig, processor);
        var inventoriesService = new InventoriesService(gameState.Inventories, gameConfig.ItemsConfig, processor);

        processor.RegisterHandler(new CmdDamageCreatureHandler(gameState, processor));
        processor.RegisterHandler(new CmdCreateInventoryHandler(gameState, gameConfig.ItemsConfig));
        processor.RegisterHandler(new CmdAddItemHandler(processor, gameConfig.ItemsConfig, inventoriesService));
        processor.RegisterHandler(new CmdAddItemInSlotHandler(gameConfig.ItemsConfig, inventoriesService));
        processor.RegisterHandler(new CmdSortStorageHandler(gameState, gameConfig.ItemsConfig));
        processor.RegisterHandler(new CmdAddSlotsToInventoryHandler());
        processor.RegisterHandler(new CmdEquipItemHandler(processor, gameState, gameConfig.ItemsConfig));
        processor.RegisterHandler(new CmdFastUnequipHandler(processor, gameState));
        processor.RegisterHandler(new CmdDeleteCreatureHandler(gameState, processor));
        processor.RegisterHandler(new CmdDeleteInventoryHandler(gameState));
        processor.RegisterHandler(new CmdKillCreatureHandler(gameState, processor));

        var inputController = sceneContainer.Resolve<GameplayInputController>();
        inputController.Bind(creaturesService.GetPlayer());

        sceneContainer.RegisterFactory(_ => creaturesService).AsSingle();
        sceneContainer.RegisterFactory(_ => inventoriesService).AsSingle();
        sceneContainer.RegisterInstance<ICommandProcessor>(processor);
        sceneContainer.RegisterInstance(AppConstants.EXIT_SCENE_REQUEST_TAG, new Subject<Unit>());
    }
}
