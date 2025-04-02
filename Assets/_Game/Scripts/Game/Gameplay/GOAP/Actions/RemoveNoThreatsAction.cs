
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using System.Linq;

public class RemoveNoThreatsAction : GoapActionBase<RemoveNoThreatsAction.Data>
{
    public override void Start(IMonoAgent agent, Data data)
    {
        data.ViewModel = agent.GetComponent<AgentBinder>().ViewModel as AgentViewModel;
    }

    public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
    {
        data.ViewModel.ThreatMap
            .Where(x => x.Value <= 0).ToList()
            .ForEach(x => data.ViewModel.ThreatMap
            .Remove(x.Key));

        return ActionRunState.Completed;
    }

    public class Data : ActionData
    {

    }
}

