using UnityEngine;

public class CmdHealCreatureHandler : ICommandHandler<CmdHealCreature>
{
    private readonly GameStateProxy _gameState;
    private readonly ICommandProcessor _commandProcessor;
    public CmdHealCreatureHandler(GameStateProxy gameState, ICommandProcessor processor)
    {
        _commandProcessor = processor;
        this._gameState = gameState;
    }
    public bool Handle(CmdHealCreature command)
    {
        var heal = Mathf.Abs(command.Heal);
        command.Creature.Stats.Health.OnNext(Mathf.Clamp(command.Creature.Stats.Health.Value + heal, 0f, command.Creature.Stats.MaxHealth.Value));

        // На случай дебаффов, которые уменьшают временно макс хп.
        // После конца дебаффа останется тот же процент хп
        command.Creature.BaseStats.Health.OnNext(command.Creature.Stats.Health.Value / command.Creature.Stats.MaxHealth.Value * command.Creature.BaseStats.MaxHealth.Value);
        return true;
    }
}