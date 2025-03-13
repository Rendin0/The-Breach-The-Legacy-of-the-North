
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class ATCFPig : ATCF<PigBrain>
{
    public override void OnCreate(AgentTypeBuilder builder)
    {
        builder.AddCapability<IdleCapabilityFactory>();
        builder.AddCapability<KillEnemyCapability>();
    }
}