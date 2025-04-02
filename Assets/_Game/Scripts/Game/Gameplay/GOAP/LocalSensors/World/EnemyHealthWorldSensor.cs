
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class EnemyHealthWorldSensor : LocalWorldSensorBase
{
    private AgentViewModel _viewModel;

    public override void Created()
    {
    }

    public override void Update()
    {
    }

    public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
    {
        _viewModel ??= references.GetCachedComponent<AgentBinder>().ViewModel as AgentViewModel;

        if (_viewModel.CurrentTarget == null)
            return false;

        return Mathf.CeilToInt(_viewModel.CurrentTarget.Stats.Health.Value);
    }
}
