
using System.Linq;

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
        if (!command.Creature.Damage(command.Damage))
            _commandProcessor.Process(new CmdKillCreature(command.Creature.CreatureId));

        return true;
    }
}