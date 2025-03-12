
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class PigBrain : AgentBrain
{
    protected override void OnInit()
    {
        provider.AgentType = goap.GetAgentType(AgentTypes.PigAgent.ToString());

        provider.RequestGoal<IdleGoal>();
    }
}
