
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public class KillEnemiesCapability : CapabilityFactory
{
    protected override string CapabilityName => nameof(KillEnemiesCapability);

    protected override void AddActions(CapabilityBuilder builder)
    {
        builder.AddAction<RemoveNoThreatsAction>()
            .AddEffect<EnemiesAmountWorldKey>(EffectType.Decrease)
            .AddCondition<EnemiesAmountWorldKey>(Comparison.GreaterThan, 0)
            .AddCondition<NoThreatTargetAmountWorldKey>(Comparison.GreaterThan, 0)
            .SetRequiresTarget(false)
            .SetBaseCost(0);

        builder.AddAction<RemoveTargetAction>()
            .AddEffect<EnemiesAmountWorldKey>(EffectType.Decrease)
            .AddCondition<EnemyHealthWorldKey>(Comparison.SmallerThanOrEqual, 0)
            .AddCondition<HaveTargetWorldKey>(Comparison.GreaterThan, 0)
            .SetRequiresTarget(false)
            .SetBaseCost(0);
        
        builder.AddAction<SetTargetEnemyAction>()
            .AddEffect<HaveTargetWorldKey>(EffectType.Increase)
            .SetBaseCost(3)
            .AddCondition<HaveTargetWorldKey>(Comparison.SmallerThanOrEqual, 0)
            .SetRequiresTarget(false)
            .AddCondition<EnemiesAmountWorldKey>(Comparison.GreaterThan, 0);


        builder.AddAction<AttackAction>()
            .SetTarget<EnemyTargetKey>()
            .AddEffect<EnemyHealthWorldKey>(EffectType.Decrease)
            .AddCondition<EnemyHealthWorldKey>(Comparison.GreaterThan, 0) 
            .AddCondition<HaveTargetWorldKey>(Comparison.GreaterThan, 0)
            .AddCondition<NoThreatTargetAmountWorldKey>(Comparison.SmallerThanOrEqual, 0)
            .SetBaseCost(1)
            .SetStoppingDistance(5f);
    }

    protected override void AddGoals(CapabilityBuilder builder)
    {
        builder.AddGoal<KillEnemiesGoal>()
            .AddCondition<EnemiesAmountWorldKey>(Comparison.SmallerThanOrEqual, 0)
            .SetBaseCost(2);
    }

    protected override void AddSensors(CapabilityBuilder builder)
    {
        builder.AddTargetSensor<EnemyTargetSensor>()
            .SetTarget<EnemyTargetKey>();

        builder.AddWorldSensor<EnemyHealthWorldSensor>()
            .SetKey<EnemyHealthWorldKey>();

        builder.AddWorldSensor<HaveTargetWorldSensor>()
            .SetKey<HaveTargetWorldKey>();

        builder.AddWorldSensor<EnemiesAmountWorldSensor>()
            .SetKey<EnemiesAmountWorldKey>();

        builder.AddWorldSensor<NoThreatTargetAmountWorldSensor>()
            .SetKey<NoThreatTargetAmountWorldKey>();
    }
}