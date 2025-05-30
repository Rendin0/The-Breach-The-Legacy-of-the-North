using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using System.Linq;

public class HaveTargetWorldSensor : LocalWorldSensorBase
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
        var highestThreat = viewModel.ThreatMap.OrderByDescending(x => x.Value).FirstOrDefault();

        return viewModel.CurrentTarget == highestThreat.Key ? 1 : 0;
    }
}