
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

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
        var viewModel = references.GetCachedComponent<CreatureBinder>().ViewModel;
        var enemiesMask = viewModel.Enemies;

        var hit = Physics2D.OverlapCircle(viewModel.Position.Value, 6f, enemiesMask);

        if (hit == null)
            return null;

        viewModel.CurrentTarget = hit.GetComponent<CreatureBinder>().ViewModel;
        return new TransformTarget(hit.transform);
    }

}