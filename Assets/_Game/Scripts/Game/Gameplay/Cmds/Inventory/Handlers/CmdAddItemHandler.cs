
public class CmdAddItemHandler : ICommandHandler<CmdAddItem>
{
    private readonly ItemsConfig _itemsConfig;
    private readonly GameStateProxy _gameState;

    public CmdAddItemHandler(ItemsConfig itemsConfig, GameStateProxy gameState)
    {
        _itemsConfig = itemsConfig;
        _gameState = gameState;
    }

    public bool Handle(CmdAddItem command)
    {



        return false;
    }
}