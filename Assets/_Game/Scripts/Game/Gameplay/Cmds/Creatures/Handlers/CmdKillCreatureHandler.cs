
public class CmdKillCreatureHandler : ICommandHandler<CmdKillCreature>
{
    private GameStateProxy _gameState;
    private ICommandProcessor _commandProcessor;

    public CmdKillCreatureHandler(GameStateProxy gameState, ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
        _gameState = gameState;
    }

    public bool Handle(CmdKillCreature command)
    {
        // TODO
        return _commandProcessor.Process(new CmdDeleteCreature(command.Id));
    }
}