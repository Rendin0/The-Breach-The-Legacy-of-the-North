
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

public class EnemiesAmountWorldSensor : LocalWorldSensorBase
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

        return viewModel.ThreatMap.Count;
    }
}