
using CrashKonijn.Agent.Core;
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
        _viewModel = references.GetCachedComponent<CreatureBinder>().ViewModel as AgentViewModel;
        var enemiesMask = _viewModel.Enemies;

        var hit = Physics2D.OverlapCircle(_viewModel.Position.Value, 6f, enemiesMask);

        if (hit == null)
            return null;

        _viewModel.CurrentTarget = hit.GetComponent<CreatureBinder>().ViewModel;
        return new TransformTarget(hit.transform);
    }

}