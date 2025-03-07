
using CrashKonijn.Goap.Runtime;

public abstract class BrainBase
{
    protected GoapActionProvider goapActionProvider { get; private set; }
    public AgentTypes AgentType { get; }
    protected BrainBase(AgentTypes agentType)
    {
        AgentType = agentType;
    }

    protected void Init(GoapActionProvider goapActionProvider)
    {
        this.goapActionProvider = goapActionProvider;
    }
}