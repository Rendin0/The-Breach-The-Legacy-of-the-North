
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using System;

public class AttackCapability : CapabilityFactoryBase
{
    public override ICapabilityConfig Create()
    {
        var builder = new CapabilityBuilder(nameof(AttackCapability));

        AddGoals(builder);
        AddActions(builder);
        AddSensors(builder);

        return builder.Build();
    }

    private void AddGoals(CapabilityBuilder builder)
    {
        builder.AddGoal<KillPlayerGoal>()
            .AddCondition<PlayerHealth>(Comparison.SmallerThanOrEqual, 0);
    }

    private void AddActions(CapabilityBuilder builder)
    {
        throw new NotImplementedException();
    }

    private void AddSensors(CapabilityBuilder builder)
    {
        throw new NotImplementedException();
    }
}