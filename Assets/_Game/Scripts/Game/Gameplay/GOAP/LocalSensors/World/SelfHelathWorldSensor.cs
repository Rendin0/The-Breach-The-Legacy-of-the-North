
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class SelfHelathPercentWorldSensor : LocalWorldSensorBase
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

        return Mathf.CeilToInt(viewModel.Stats.Health.Value / viewModel.Stats.MaxHealth.Value * 100f);
    }
}