
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public class PigAgentTypeFactory : AgentTypeFactoryBase
{
    public override IAgentTypeConfig Create()
    {
        var builder = new AgentTypeBuilder("PigAgent");

        builder.AddCapability<IdleCapabilityFactory>();

        return builder.Build();
    }
}