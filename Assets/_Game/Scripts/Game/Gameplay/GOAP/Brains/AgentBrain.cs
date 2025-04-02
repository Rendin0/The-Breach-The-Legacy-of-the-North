using System.Linq;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public abstract class AgentBrain : MonoBehaviour
{
    protected AgentBehaviour agentBehaviour;
    protected GoapActionProvider provider;
    protected GoapBehaviour goap;
    protected AgentViewModel agent;

    public abstract string AgentType { get; }
    public void Init(AgentBehaviour agentBehaviour, GoapActionProvider goapActionProvider, GoapBehaviour goapBehaviour, AgentViewModel agent)
    {
        this.agentBehaviour = agentBehaviour;
        this.provider = goapActionProvider;
        this.goap = goapBehaviour;
        this.agent = agent;

        provider.AgentType = goap.GetAgentType(AgentType);

        OnInit();
    }

    protected void ResolveCurrentTarget()
    {
        if (agent.ThreatMap.Count == 0)
        {
            agentBehaviour.StopAction();
            provider.RequestGoal<IdleGoal>();

            agent.CurrentTarget = null;

            return;
        }

        agent.CurrentTarget = agent.ThreatMap.OrderByDescending(x => x.Value).First().Key;
    }

    protected abstract void OnInit();
}