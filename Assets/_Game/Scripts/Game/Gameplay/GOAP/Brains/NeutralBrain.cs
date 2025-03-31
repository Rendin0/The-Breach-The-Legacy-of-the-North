
public class NeutralBrain : AgentBrain
{
    public override string AgentType => AgentTypes.NeutralAgent.ToString();

    protected override void OnInit()
    {
        provider.RequestGoal<KillEnemyGoal>();
    }
}