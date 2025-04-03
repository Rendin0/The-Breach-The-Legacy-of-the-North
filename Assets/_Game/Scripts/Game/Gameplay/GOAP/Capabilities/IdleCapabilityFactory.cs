
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public class IdleCapability : CapabilityFactory
{
    protected override string CapabilityName => nameof(IdleCapability);

    protected override void AddGoals(CapabilityBuilder builder)
    {
        builder.AddGoal<IdleGoal>()
            .AddCondition<IsIdleWorldKey>(Comparison.GreaterThanOrEqual, 1)
            .SetBaseCost(50);
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