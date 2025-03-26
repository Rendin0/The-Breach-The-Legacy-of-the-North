
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using System;

public abstract class CapabilityFactory : CapabilityFactoryBase
{
    protected abstract string CapabilityName { get; }
    public override ICapabilityConfig Create()
    {
        var builder = new CapabilityBuilder(CapabilityName);

        AddGoals(builder);
        AddActions(builder);
        AddSensors(builder);

        return builder.Build();
    }
    protected abstract void AddGoals(CapabilityBuilder builder);
    protected abstract void AddActions(CapabilityBuilder builder);
    protected abstract void AddSensors(CapabilityBuilder builder);
}