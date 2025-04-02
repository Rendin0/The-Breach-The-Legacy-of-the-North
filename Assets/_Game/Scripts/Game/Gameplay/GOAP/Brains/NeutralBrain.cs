using ObservableCollections;
using R3;

public class NeutralBrain : AgentBrain
{
    public override string AgentType => AgentTypes.NeutralAgent.ToString();

    protected override void OnInit()
    {
        provider.RequestGoal<IdleGoal>();
        agent.ThreatMap.ObserveAdd().Subscribe(_ => ResolveCurrentGoal());
        agent.ThreatMap.ObserveRemove().Subscribe(_ => ResolveCurrentGoal());
    }
}