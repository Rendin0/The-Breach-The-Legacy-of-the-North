
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class WanderTargetSensor : LocalTargetSensorBase
{
    public override void Created()
    {
    }

    public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget existingTarget)
    {
        Vector2 position = GetRandomPosition(agent);


        return new PositionTarget(position);
    }

    private Vector2 GetRandomPosition(IActionReceiver agent)
    {
        // 10 попыток найти подходящую позицию
        for (int count = 0; count < 10; count++)
        {
            Vector2 randomPos = Random.insideUnitCircle * 5f;
            Vector2 position = (Vector2)agent.Transform.position + randomPos;

            if (NavMesh.SamplePosition(position, out var hit, 1, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return agent.Transform.position;
    }

    public override void Update()
    {

    }
}