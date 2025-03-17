
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class AttackAction : GoapActionBase<AttackAction.Data>
{
    public override void Start(IMonoAgent agent, Data data)
    {
        data.ViewModel = agent.GetComponent<CreatureBinder>().ViewModel;
        data.Timer = 1f;
    }

    public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
    {
        if (data.Timer <= 0f)
        {
            return ActionRunState.Completed;
        }
        data.Timer -= context.DeltaTime;

        bool inRange = Vector2.Distance(agent.transform.position, data.ViewModel.CurrentTarget.Position.Value) <= 1.5f;

        if (inRange)
        {

            data.ViewModel.Attack(data.ViewModel.CurrentTarget.Position.Value);
        }


        return ActionRunState.Continue;
    }

    public class Data : ActionData
    {
        public CreatureViewModel ViewModel { get; set; }
    }
}
