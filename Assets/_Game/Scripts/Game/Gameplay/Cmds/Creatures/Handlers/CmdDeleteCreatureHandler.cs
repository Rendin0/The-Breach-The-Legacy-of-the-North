
using System.Linq;

public class CmdDeleteCreatureHandler : ICommandHandler<CmdDeleteCreature>
{
    private GameStateProxy _gameState;

    public CmdDeleteCreatureHandler(GameStateProxy gameState)
    {
        _gameState = gameState;
    }

    public bool Handle(CmdDeleteCreature command)
    {
        var creature = _gameState.Creatures.FirstOrDefault(c => command.EntityId == c.Id);

        if (creature != null)
        {
            _gameState.Creatures.Remove(creature);
            return true;
        }


        return false;
    }
}