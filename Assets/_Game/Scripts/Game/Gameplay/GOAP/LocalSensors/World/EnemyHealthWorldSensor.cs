
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class EnemyHealthWorldSensor : LocalWorldSensorBase
{

    public override void Created()
    {
    }

    public override void Update()
    {
    }

    public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
    {
        var viewModel = references.GetCachedComponent<AgentBinder>().ViewModel as AgentViewModel;

        if (viewModel.CurrentTarget == null)
            return false;

        return Mathf.CeilToInt(viewModel.CurrentTarget.Stats.Health.Value);
    }
}
