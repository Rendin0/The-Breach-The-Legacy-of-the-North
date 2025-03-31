public class PeacefulBrain : AgentBrain
{
    public override string AgentType => AgentTypes.PeacefulAgent.ToString();

    protected override void OnInit()
    {
        provider.RequestGoal<IdleGoal>();
    }
}
