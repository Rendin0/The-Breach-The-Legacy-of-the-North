
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public class WanderCapability : CapabilityFactoryBase
{
    public override ICapabilityConfig Create()
    {
        var builder = new CapabilityBuilder("WanderCapability");

        AddGoals(builder);
        AddActions(builder);
        AddSensors(builder);

        return builder.Build();
    }

    private void AddGoals(CapabilityBuilder builder)
    {
        builder.AddGoal<WanderGoal>()
            .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);
    }

    private void AddActions(CapabilityBuilder builder)
    {
        builder.AddAction<WanderAction>()
            .SetTarget<WanderTarget>()
            .AddEffect<IsWandering>(EffectType.Increase)
            .SetBaseCost(5)
            .SetStoppingDistance(10);
    }

    private void AddSensors(CapabilityBuilder builder)
    {
        builder.AddTargetSensor<WanderTargetSensor>()
            .SetTarget<WanderTarget>();
    }
}
