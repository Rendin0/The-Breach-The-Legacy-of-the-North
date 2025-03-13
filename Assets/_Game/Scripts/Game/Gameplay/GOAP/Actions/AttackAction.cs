
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;

public class AttackAction : GoapActionBase<AttackAction.Data>
{
    public override void Start(IMonoAgent agent, Data data)
    {
        data.Timer = 1f;

    }

    public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
    {
        if (data.Timer <= 0f)
            return ActionRunState.Completed;

        data.Timer -= context.DeltaTime;



        return ActionRunState.Continue;
    }

    public class Data : ActionData { }
}
