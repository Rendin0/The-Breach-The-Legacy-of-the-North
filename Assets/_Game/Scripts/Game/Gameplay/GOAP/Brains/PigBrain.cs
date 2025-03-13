
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public class PigBrain : AgentBrain
{
    public override string AgentType => AgentTypes.PigAgent.ToString();

    protected override void OnInit()
    {
        provider.AgentType = goap.GetAgentType(AgentType);

        provider.RequestGoal<IdleGoal>();
    }
}
