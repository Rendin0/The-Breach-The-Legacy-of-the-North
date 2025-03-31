
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class EnemyTargetSensor : LocalTargetSensorBase
{
    private AgentViewModel _viewModel;

    public override void Created()
    {
    }

    public override void Update()
    {
    }

    public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget existingTarget)
    {
        _viewModel = references.GetCachedComponent<AgentBinder>().ViewModel as AgentViewModel;

        if (_viewModel.CurrentTarget == null)
            return null;

        return new TransformTarget(_viewModel.CurrentTarget.Transform);
    }

}