
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public class KillEnemyCapability : CapabilityFactory
{
    protected override string CapabilityName => nameof(KillEnemyCapability);

    protected override void AddActions(CapabilityBuilder builder)
    {
        builder.AddAction<AttackAction>()
            .SetTarget<EnemyTargetKey>()
            .AddEffect<EnemyHealthWorldKey>(EffectType.Decrease)
            .SetBaseCost(1)
            .SetStoppingDistance(5f);
    }

    protected override void AddGoals(CapabilityBuilder builder)
    {
        builder.AddGoal<KillEnemyGoal>()
            .AddCondition<EnemyHealthWorldKey>(Comparison.SmallerThanOrEqual, 0)
            .SetBaseCost(4);
    }

    protected override void AddSensors(CapabilityBuilder builder)
    {
        builder.AddTargetSensor<EnemyTargetSensor>()
            .SetTarget<EnemyTargetKey>();

        builder.AddWorldSensor<EnemyHealthWorldSensor>()
            .SetKey<EnemyHealthWorldKey>();
    }
}