using CrashKonijn.Goap.Runtime;

public class ATCFNeutral : ATCF<NeutralBrain>
{
    public override void OnCreate(AgentTypeBuilder builder)
    {
        builder.AddCapability<IdleCapabilityFactory>();
        builder.AddCapability<KillEnemyCapability>();
    }
}
