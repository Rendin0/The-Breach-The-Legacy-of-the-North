
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public class SelfSustainCapability : CapabilityFactory
{
    protected override string CapabilityName => nameof(SelfSustainCapability);

    protected override void AddActions(CapabilityBuilder builder)
    {
        builder.AddAction<HealSelfAction>()
            .SetTarget<SafePositionTargetKey>()
            .SetBaseCost(0)
            .AddEffect<SelfHelathPercentWorldKey>(EffectType.Increase)
            .AddCondition<SelfHelathPercentWorldKey>(Comparison.SmallerThanOrEqual, 20);
    }

    protected override void AddGoals(CapabilityBuilder builder)
    {
        builder.AddGoal<SustainSelfGoal>()
            .AddCondition<SelfHelathPercentWorldKey>(Comparison.GreaterThanOrEqual, 40)
            .SetBaseCost(0);
    }

    protected override void AddSensors(CapabilityBuilder builder)
    {
        builder.AddWorldSensor<SelfHelathPercentWorldSensor>()
            .SetKey<SelfHelathPercentWorldKey>();

        builder.AddTargetSensor<SafePositionTargetSensor>()
            .SetTarget<SafePositionTargetKey>();
    }
}