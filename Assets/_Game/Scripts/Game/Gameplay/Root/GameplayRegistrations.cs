using R3;

public static class GameplayRegistrations
{

    public static void Register(DIContainer sceneContainer, GameplayEnterParams enterParams)
    {
        var gameStateProvider = sceneContainer.Resolve<IGameStateProvider>();
        var gameState = gameStateProvider.GameState;
        var processor = new CommandProcessor(gameStateProvider);
        processor.RegisterHandler(new CmdDamageCreatureHandler(gameState));
        processor.RegisterHandler(new CmdCreateCreatureHandler(gameState));


        sceneContainer.RegisterFactory(_ => new CreaturesSerivce(gameState.Creatures, processor)).AsSingle();
        sceneContainer.RegisterInstance<ICommandProcessor>(processor);
        sceneContainer.RegisterInstance(AppConstants.EXIT_SCENE_REQUEST_TAG, new Subject<Unit>());
    }
}
