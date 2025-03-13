
using System.Collections.Generic;

public class CmdCreateCreatureHandler : ICommandHandler<CmdCreateCreature>
{
    private readonly GameStateProxy _gameState;
    private readonly Dictionary<string, CreatureConfig> _configs = new();

    public CmdCreateCreatureHandler(GameStateProxy gameState, CreaturesConfig configs)
    {
        _gameState = gameState;

        foreach (var config in configs.Creatures)
        {
            _configs[config.TypeId] = config;
        }
    }

    public bool Handle(CmdCreateCreature command)
    {
        var entityId = _gameState.GetEntityId();
        var creatureConfig = _configs[command.TypeId];

        var creature = new CreatureEntity(creatureConfig.Stats)
        {
            Id = entityId,
            TypeId = command.TypeId,
            Position = command.Position,
            AgentType = creatureConfig.AgentType,
        };

        var creatureProxy = new CreatureEntityProxy(creature);
        _gameState.Creatures.Add(creatureProxy);


        return true;
    }
}