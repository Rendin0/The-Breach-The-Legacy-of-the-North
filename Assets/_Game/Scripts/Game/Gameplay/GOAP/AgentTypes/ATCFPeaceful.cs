
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class ATCFPeaceful : ATCF<PeacefulBrain>
{
    public override void OnCreate(AgentTypeBuilder builder)
    {
        builder.AddCapability<IdleCapabilityFactory>();
    }
}