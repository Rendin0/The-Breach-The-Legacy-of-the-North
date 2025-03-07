
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class PlayerTargetSensor : LocalTargetSensorBase, IInjectable
{
    private AttackConfigSO _attackConfig;

    public override void Created()
    {
    }

    public void Inject(DependencyInjector injector)
    {
        _attackConfig = injector.AttagConfig;
    }

    public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget existingTarget)
    {
        var collider = Physics2D.OverlapCircle(agent.Transform.position, _attackConfig.SensorRadius, _attackConfig.AttackLayer);
        if (collider != null)
            return new TransformTarget(collider.transform);

        return null;
    }

    public override void Update()
    {
    }
}
