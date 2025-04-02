using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;

public class SetTargetEnemyAction : GoapActionBase<SetTargetEnemyAction.Data>
{
    public override void Start(IMonoAgent agent, Data data)
    {
        data.ViewModel = agent.GetComponent<AgentBinder>().ViewModel as AgentViewModel;

    }

    public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
    {
        var pair = data.ViewModel.ThreatMap.OrderByDescending(pair => pair.Value).First();
        data.ViewModel.CurrentTarget = pair.Key;

        return ActionRunState.Completed;
    }



    public class Data : ActionData
    {
        public AgentViewModel ViewModel { get; set; }
    }
}