using UnityEngine;

public class CmdDamageCreatureHandler : ICommandHandler<CmdDamageCreature>
{
    private readonly GameStateProxy _gameState;
    private readonly ICommandProcessor _commandProcessor;

    public CmdDamageCreatureHandler(GameStateProxy gameState, ICommandProcessor processor)
    {
        _commandProcessor = processor;
        this._gameState = gameState;
    }

    public bool Handle(CmdDamageCreature command)
    {
        float damageResult = CalculateDamage(command);
        AddThreat(command, damageResult);

        return damageResult > 0f;
    }

    private float CalculateDamage(CmdDamageCreature command)
    {
        bool isAlive = true;
        float damageResult = 0f;
        if (!command.Creature.Stats.Immortal.Value)
        {
            // Физическая часть
            damageResult = Mathf.Abs(command.Damage.PhysicalData) * (1f - (command.Creature.DynamicStats.PhysicalDamageResistance / 100f));

            // Магическая часть
            damageResult += Mathf.Abs(command.Damage.MagicalData) * (1f - (command.Creature.DynamicStats.MagicalDamageResistance / 100f));

            // Доп уменьшение урона 
            damageResult *= 1f - command.Creature.Stats.DamageResistance.Value;

            command.Creature.Stats.Health.OnNext(command.Creature.Stats.Health.Value - damageResult);

            // На случай дебаффов, которые уменьшают временно макс хп.
            // После конца дебаффа останется тот же процент хп
            command.Creature.BaseStats.Health.OnNext(command.Creature.Stats.Health.Value / command.Creature.Stats.MaxHealth.Value * command.Creature.BaseStats.MaxHealth.Value);

            // Для способностей Delayed reckoning, Unbreakable, считает кол-во урона за последние 5 секунд
            command.Creature.DynamicStats.HealthChanges += damageResult;
            command.Creature.DynamicStats.HealthChangesTimer(damageResult);
            isAlive = command.Creature.Stats.Health.Value > 0;
        }


        if (!isAlive)
            command.Creature.CreatureRequests.KillRequest.OnNext(command.Creature);

        return damageResult;
    }
    private void AddThreat(CmdDamageCreature command, float damage)
    {
        float threat = damage;

        if (command.Creature is AgentViewModel agent)
        {
            agent.ThreatMap[command.DamageDealer] = threat;
        }
    }
}

