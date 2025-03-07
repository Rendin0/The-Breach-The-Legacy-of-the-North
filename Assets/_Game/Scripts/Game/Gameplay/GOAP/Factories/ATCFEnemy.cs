
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public class ATCFEnemy : ATCF
{
    public override AgentTypes AgentType => AgentTypes.Enemy;

    public override IAgentTypeConfig Create()
    {
        AgentTypeBuilder builder = new(AgentType.ToString());

        builder.AddCapability<WanderCapability>();
        builder.AddCapability<AttackCapability>();

        return builder.Build();
    }

    public override BrainBase GetBrain()
    {
        return new EnemyBrain(AgentType);
    }
}