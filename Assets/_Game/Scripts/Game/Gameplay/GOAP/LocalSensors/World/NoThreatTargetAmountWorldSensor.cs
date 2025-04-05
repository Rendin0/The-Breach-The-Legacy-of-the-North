
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using System.Linq;

public class NoThreatTargetAmountWorldSensor : LocalWorldSensorBase
{
    public override ISensorTimer Timer => SensorTimer.Interval(1f);
    public override void Created()
    {
    }

    public override void Update()
    {
    }

    public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
    {
        var viewModel = references.GetCachedComponent<AgentBinder>().ViewModel as AgentViewModel;

        var noThreat = viewModel.ThreatMap.Where(pair => pair.Value <= 0f).ToList();

        return new SenseValue(noThreat.Count);
    }
}