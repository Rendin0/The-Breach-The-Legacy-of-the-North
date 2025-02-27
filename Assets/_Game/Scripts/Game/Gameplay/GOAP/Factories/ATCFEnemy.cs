
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public class ATCFEnemy : AgentTypeFactoryBase
{
    public override IAgentTypeConfig Create()
    {
        AgentTypeBuilder builder = new("Enemy");

        builder.AddCapability<WanderCapability>();

        return builder.Build();
    }

}