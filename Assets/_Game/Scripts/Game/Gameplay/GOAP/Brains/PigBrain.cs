
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class PigBrain : AgentBrain
{
    protected override void OnInit()
    {
        provider.AgentType = goap.GetAgentType("PigAgent");

        provider.RequestGoal<IdleGoal>();
    }
}
