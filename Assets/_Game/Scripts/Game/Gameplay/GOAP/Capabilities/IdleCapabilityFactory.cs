
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public class IdleCapabilityFactory : CapabilityFactory
{
    protected override string CapabilityName => nameof(IdleCapabilityFactory);

    protected override void AddGoals(CapabilityBuilder builder)
    {
        builder.AddGoal<IdleGoal>()
            .AddCondition<IsIdleWorldKey>(Comparison.GreaterThanOrEqual, 1)
            .SetBaseCost(2);
    }

    protected override void AddActions(CapabilityBuilder builder)
    {
        builder.AddAction<IdleAction>()
            .AddEffect<IsIdleWorldKey>(EffectType.Increase)
            .SetTarget<IdleTargetKey>();
    }

    protected override void AddSensors(CapabilityBuilder builder)
    {
        builder.AddTargetSensor<IdleTargetSensor>()
            .SetTarget<IdleTargetKey>();
    }
}