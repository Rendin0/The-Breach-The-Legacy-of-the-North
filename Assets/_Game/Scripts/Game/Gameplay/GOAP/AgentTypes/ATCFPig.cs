
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class ATCFPig : ATCF
{
    public override string AgentType => PigBrain.AgentType;

    public override AgentBrain GetBrain(GameObject go)
    {
        return go.AddComponent<PigBrain>();
    }

    public override void OnCreate(AgentTypeBuilder builder)
    {
        builder.AddCapability<IdleCapabilityFactory>();
    }
}