using ObservableCollections;
using R3;

public class NeutralBrain : AgentBrain
{
    public override string AgentType => AgentTypes.NeutralAgent.ToString();

    protected override void OnInit()
    {
        provider.RequestGoal<IdleGoal>();
        agent.ThreatMap.ObserveAdd().Subscribe(pair => ThreatAdded(pair.Value.Key, pair.Value.Value));
        agent.ThreatMap.ObserveRemove().Subscribe(pair => ThreatRemoved(pair.Value.Key, pair.Value.Value));
        agent.ThreatMap.ObserveChanged().Subscribe(pair => ThreatChanged(pair.NewItem.Key, pair.NewItem.Value));
    }

    private void ThreatChanged(CreatureViewModel key, float value)
    {
        ResolveCurrentTarget();
    }

    private void ThreatAdded(CreatureViewModel creature, float threat)
    {
        ResolveCurrentTarget();

        agentBehaviour.StopAction();
        provider.RequestGoal<KillEnemyGoal>();
    }

    private void ThreatRemoved(CreatureViewModel creature, float threat)
    {
        ResolveCurrentTarget();
    }
}