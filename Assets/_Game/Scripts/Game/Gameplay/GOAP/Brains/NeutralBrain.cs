using R3;
using ObservableCollections;

public class NeutralBrain : AgentBrain
{
    public override string AgentType => AgentTypes.NeutralAgent.ToString();

    protected override void OnInit()
    {
        provider.RequestGoal<IdleGoal>();
        agent.ThreatMap.ObserveAdd().Subscribe(pair => ThreatAdded(pair.Value.Key, pair.Value.Value));
        agent.ThreatMap.ObserveRemove().Subscribe(pair => ThreatRemoved(pair.Value.Key, pair.Value.Value));
    }

    private void ThreatAdded(CreatureViewModel creature, float threat)
    {
        provider.RequestGoal<KillEnemyGoal>();
    }

    private void ThreatRemoved(CreatureViewModel creature, float threat)
    {
        agentBehaviour.StopAction();
        provider.RequestGoal<IdleGoal>();
    }
}