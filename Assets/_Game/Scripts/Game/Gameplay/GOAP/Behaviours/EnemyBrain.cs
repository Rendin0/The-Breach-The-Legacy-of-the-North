
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class EnemyBrain : BrainBase
{
    public EnemyBrain(AgentTypes AgentType)
        : base(AgentType)
    {
        goapActionProvider.RequestGoal<WanderGoal>(false);
    }
}