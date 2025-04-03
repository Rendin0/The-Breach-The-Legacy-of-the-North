
using CrashKonijn.Goap.Runtime;

public class ATCFAggressive : ATCF<AggressiveBrain>
{
    public override void OnCreate(AgentTypeBuilder builder)
    {
        builder.AddCapability<IdleCapability>();
        builder.AddCapability<KillEnemiesCapability>();
        builder.AddCapability<SelfSustainCapability>();
    }
}