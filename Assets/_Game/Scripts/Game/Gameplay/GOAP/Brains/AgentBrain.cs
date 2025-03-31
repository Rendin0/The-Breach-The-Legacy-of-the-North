using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using System.Collections.Generic;
using System.Linq;
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

        var enumerator = agent.ThreatMap.GetEnumerator();
        enumerator.MoveNext();
        var maxThreat = enumerator.Current;

        foreach (var threat in agent.ThreatMap)
        {
            if (threat.Value > maxThreat.Value)
                maxThreat = threat;
        }

        agent.CurrentTarget = maxThreat.Key;
    }

    protected abstract void OnInit();
}