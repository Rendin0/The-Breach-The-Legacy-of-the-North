
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;

public class RemoveTargetAction : GoapActionBase<RemoveTargetAction.Data>
{
    public override void Start(IMonoAgent agent, Data data)
    {
        data.ViewModel = agent.GetComponent<AgentBinder>().ViewModel as AgentViewModel;
    }

    public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
    {
        var creature = data.ViewModel.CurrentTarget;
        data.ViewModel.ThreatMap.Remove(creature);
        data.ViewModel.CurrentTarget = null;

        return ActionRunState.Completed;
    }

    public class Data : ActionData
    {
        public AgentViewModel ViewModel { get; set; }
    }

}