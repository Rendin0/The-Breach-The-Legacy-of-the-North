using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public abstract class ATCF : AgentTypeFactoryBase
{
    public abstract string AgentType { get; }

    // Do not override Create()
    public override IAgentTypeConfig Create()
    {
        var builder = new AgentTypeBuilder(AgentType);

        OnCreate(builder);

        return builder.Build();
    }

    // Need to override
    public abstract void OnCreate(AgentTypeBuilder builder);

    public abstract AgentBrain GetBrain(GameObject go);
}
