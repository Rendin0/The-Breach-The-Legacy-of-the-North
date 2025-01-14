
using System.Linq;

public class CmdHandlerDamageCreature : ICommandHandler<CmdDamageCreature>
{
    private readonly GameStateProxy _gameState;

    public CmdHandlerDamageCreature(GameStateProxy gameState)
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