
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;

public class EnemyTargetSensor : LocalTargetSensorBase
{
    public override void Created()
    {
    }

    public override void Update()
    {
    }

    public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget existingTarget)
    {
        var viewModel = references.GetCachedComponent<AgentBinder>().ViewModel as AgentViewModel;

        if (viewModel.CurrentTarget == null)
            return null;

        return new TransformTarget(viewModel.CurrentTarget.Transform);
    }

}