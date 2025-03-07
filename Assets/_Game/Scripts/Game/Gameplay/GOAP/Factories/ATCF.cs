
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public abstract class ATCF : AgentTypeFactoryBase
{
    public abstract AgentTypes AgentType { get; }
    public abstract BrainBase GetBrain();
    public override abstract IAgentTypeConfig Create();
}