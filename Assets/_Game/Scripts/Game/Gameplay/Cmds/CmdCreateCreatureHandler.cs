
public class CmdCreateCreatureHandler : ICommandHandler<CmdCreateCreature>
{
    private readonly GameStateProxy _gameState;

    public CmdCreateCreatureHandler(GameStateProxy gameState)
    {
        _gameState = gameState;
    }

    public bool Handle(CmdCreateCreature command)
    {
        var entityId = _gameState.GetEntityId();
        var creature = new CreatureEntity()
        {
            Id = entityId,
            TypeId = command.TypeId,
            MaxHealth = command.MaxHealth,
            Health = command.Health,
            Position = command.Position
        };

        var creatureProxy = new CreatureEntityProxy(creature);

        _gameState.Creatures.Add(creatureProxy);

        return true;
    }
}