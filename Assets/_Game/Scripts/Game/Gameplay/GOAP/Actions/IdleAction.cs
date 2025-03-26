
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;

public class IdleAction : GoapActionBase<IdleAction.Data>
{
    public override void Start(IMonoAgent agent, Data data)
    {
        data.Timer = 3f;
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
