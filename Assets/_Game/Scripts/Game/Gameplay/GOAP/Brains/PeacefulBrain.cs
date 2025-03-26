public class PeacefulBrain : AgentBrain
{
    public override string AgentType => AgentTypes.PigAgent.ToString();

    protected override void OnInit()
    {
        provider.AgentType = goap.GetAgentType(AgentType);

        provider.RequestGoal<IdleGoal>();
    }
}
