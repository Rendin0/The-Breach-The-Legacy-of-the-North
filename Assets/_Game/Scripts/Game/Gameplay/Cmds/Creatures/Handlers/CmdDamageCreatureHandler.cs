public class CmdDamageCreatureHandler : ICommandHandler<CmdDamageCreature>
{
    private readonly GameStateProxy _gameState;
    private readonly ICommandProcessor _commandProcessor;

    public CmdDamageCreatureHandler(GameStateProxy gameState, ICommandProcessor processor)
    {
        _commandProcessor = processor;
        this._gameState = gameState;
    }

    public bool Handle(CmdDamageCreature command)
    {
        command.Creature.Damage(command.Damage);

        return true;
    }
}