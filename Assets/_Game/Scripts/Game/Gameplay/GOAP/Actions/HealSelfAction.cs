
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;

public class HealSelfAction : GoapActionBase<HealSelfAction.Data>
{

    public override void Start(IMonoAgent agent, Data data)
    {
        data.ViewModel = agent.GetComponent<AgentBinder>().ViewModel as AgentViewModel;
    }

    public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
    {
        data.ViewModel.UseHeal(0);

        return ActionRunState.Completed;
    }


    public class Data : ActionData
    {
    }
}


