
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public class PigAgentTypeFactory : AgentTypeFactoryBase
{
    public override IAgentTypeConfig Create()
    {
        var builder = new AgentTypeBuilder(AgentTypes.PigAgent.ToString());

        builder.AddCapability<IdleCapabilityFactory>();

        return builder.Build();
    }
}