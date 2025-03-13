
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class EnemyHealthWorldSensor : LocalWorldSensorBase
{
    private CreatureViewModel _viewModel;

    public override void Created()
    {
    }

    public override void Update()
    {
    }

    public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
    {
        _viewModel ??= references.GetCachedComponent<CreatureBinder>().ViewModel;
        var enemiesMask = _viewModel.Enemies;

        var hit = Physics2D.OverlapCircle(_viewModel.Position.Value, 5f, enemiesMask);
        if (hit == null)
            return 0;

        _viewModel.CurrentTarget = hit.GetComponent<CreatureBinder>().ViewModel;
        return Mathf.CeilToInt(_viewModel.CurrentTarget.Stats.Health.Value);
    }
}