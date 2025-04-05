using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class SafePositionTargetSensor : LocalTargetSensorBase
{
    public override void Created()
    {
    }

    public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget existingTarget)
    {
        var viewModel = references.GetCachedComponent<AgentBinder>().ViewModel as AgentViewModel;

        if (viewModel.CurrentTarget == null)
            return new PositionTarget(agent.Transform.position);


        var enemyTransform = viewModel.CurrentTarget.Transform;

        Vector3 safePos = GetSafePosition(agent, enemyTransform);

        return new PositionTarget(safePos);
    }

    private Vector3 GetSafePosition(IActionReceiver agent, Transform enemyTransform)
    {
        // 20 попыток найти безопасную позицию
        for (int i = 0; i < 20; i++)
        {
            var random = Random.insideUnitCircle * 10f;
            var position = enemyTransform.position + (Vector3)random;

            if (NavMesh.SamplePosition(position, out var hit, 1, NavMesh.AllAreas))
            {
                var pos = new Vector3(hit.position.x, hit.position.y, agent.Transform.position.z);

                // Нашли позицию, но она слишком близко
                if (Vector2.Distance(pos, enemyTransform.position) < 8f)
                    continue;

                return pos;
            }
        }

        return agent.Transform.position;
    }

    public override void Update()
    {
    }
}