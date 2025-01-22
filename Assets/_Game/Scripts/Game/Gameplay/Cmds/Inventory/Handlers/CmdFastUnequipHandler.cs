
public class CmdFastUnequipHandler : ICommandHandler<CmdFastUnequip>
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly GameStateProxy _gameStateProxy;

    public CmdFastUnequipHandler(ICommandProcessor commandProcessor, GameStateProxy gameStateProxy)
    {
        _commandProcessor = commandProcessor;
        _gameStateProxy = gameStateProxy;
    }

    public bool Handle(CmdFastUnequip command)
    {
        foreach (var equip in command.Inventory.Equipment)
        {
            var cmd = new CmdAddItem(command.Inventory.OwnerId, equip.Value.ItemId.Value, equip.Value.Amount.Value);

            // Нету места
            if (!_commandProcessor.Process(cmd))
            {
                equip.Value.Amount.OnNext(cmd.Amount);
                return false;
            }

            equip.Value.Amount.OnNext(0);
            equip.Value.ItemId.OnNext(ItemsIDs.Nothing);
        }

        return true;

    }
}