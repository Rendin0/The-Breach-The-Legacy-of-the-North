
using System.Linq;

public class CmdDeleteInventoryHandler : ICommandHandler<CmdDeleteInventory>
{
    private GameStateProxy _gameState;

    public CmdDeleteInventoryHandler(GameStateProxy gameState)
    {
        _gameState = gameState;
    }

    public bool Handle(CmdDeleteInventory command)
    {
        var inventory = _gameState.Inventories.FirstOrDefault(i => i.OwnerId == command.OwnerId);

        if (inventory == null)
            return false;

        _gameState.Inventories.Remove(inventory);
        return true;
    }
}

