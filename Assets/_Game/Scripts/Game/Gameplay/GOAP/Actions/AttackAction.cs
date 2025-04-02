
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class AttackAction : GoapActionBase<AttackAction.Data>
{
    public override void Start(IMonoAgent agent, Data data)
    {
        data.ViewModel = agent.GetComponent<AgentBinder>().ViewModel as AgentViewModel;
        data.Timer = 1f;
    }

    public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
    {
        if (data.Timer <= 0f)
        {
            return ActionRunState.Completed;
        }
        data.Timer -= context.DeltaTime;

        bool inRange = Vector2.Distance(agent.transform.position, data.ViewModel.CurrentTarget.Position.Value) <= 2f;

        if (inRange)
        {
            int randomAbility = Random.Range(0, data.ViewModel.Abilities.Count);

            data.ViewModel.UseAbility(randomAbility, data.ViewModel.CurrentTarget.Position.Value);
        }


        return ActionRunState.Continue;
    }

    public class Data : ActionData
    {
    }
}
