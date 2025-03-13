
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class EnemyTargetSensor : LocalTargetSensorBase
{
    private CreatureViewModel _viewModel = null;

    public override void Created()
    {
    }

    public override void Update()
    {
    }

    public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget existingTarget)
    {
        _viewModel ??= references.GetCachedComponent<CreatureBinder>().ViewModel;
        var enemiesMask = _viewModel.Enemies;

        var hit = Physics2D.OverlapCircle(_viewModel.Position.Value, 5f, enemiesMask);

        if (hit == null)
            return null;

        _viewModel.CurrentTarget = hit.GetComponent<CreatureBinder>().ViewModel;
        return new PositionTarget(hit.transform.position);
    }

}