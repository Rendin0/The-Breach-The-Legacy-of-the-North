using CrashKonijn.Goap.Runtime;

public class ATCFPeaceful : ATCF<PeacefulBrain>
{
    public override void OnCreate(AgentTypeBuilder builder)
    {
        builder.AddCapability<IdleCapabilityFactory>();
    }
}