
using System.Linq;

public class CmdDamageCreatureHandler : ICommandHandler<CmdDamageCreature>
{
    private readonly GameStateProxy _gameState;

    public CmdDamageCreatureHandler(GameStateProxy gameState)
    {
        this._gameState = gameState;
    }

    public bool Handle(CmdDamageCreature command)
    {
        var creature = _gameState.Creatures.FirstOrDefault(c => c.Id == command.CreatureId);
        if (creature != null)
        {
            creature.Health.OnNext(creature.Health.Value - command.Damage);

            return true;
        }

        return false;
    }
}