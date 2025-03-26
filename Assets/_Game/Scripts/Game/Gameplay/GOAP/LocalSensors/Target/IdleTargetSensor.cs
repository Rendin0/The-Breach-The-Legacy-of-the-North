
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class IdleTargetSensor : LocalTargetSensorBase
{
    public override void Created()
    {
    }
    public override void Update()
    {
    }

    public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget existingTarget)
    {
        var randomPos = GetRandomPosition(agent);

        return new PositionTarget(randomPos);
    }

    private Vector3 GetRandomPosition(IActionReceiver agent)
    {
        for (int count = 0; count < 10; count++)
        {
            var random = Random.insideUnitCircle * 5f;
            var position = agent.Transform.position + (Vector3)random;

            if (NavMesh.SamplePosition(position, out var hit, 1, NavMesh.AllAreas))
            {
                return new(hit.position.x, hit.position.y, agent.Transform.position.z);
            }
        }

        return agent.Transform.position;
    }

}