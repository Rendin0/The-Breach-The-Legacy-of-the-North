
using System.Linq;

public class CmdDeleteCreatureHandler : ICommandHandler<CmdDeleteCreature>
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly GameStateProxy _gameState;

    public CmdDeleteCreatureHandler(GameStateProxy gameState, ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
        _gameState = gameState;
    }

    public bool Handle(CmdDeleteCreature command)
    {
        var cmdDeleteInventory = new CmdDeleteInventory(command.EntityId);
        _commandProcessor.Process(cmdDeleteInventory);

        var creature = _gameState.Creatures.FirstOrDefault(c => command.EntityId == c.Id);

        if (creature != null)
        {
            _gameState.Creatures.Remove(creature);
            return true;
        }


        return false;
    }
}