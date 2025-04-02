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

    protected void ResolveCurrentGoal()
    {
        agentBehaviour.StopAction();
        
        if (agent.ThreatMap.Count == 0)
        {
            provider.RequestGoal<IdleGoal>();
            return;
        }

        provider.RequestGoal<KillEnemiesGoal>();
    }

    protected abstract void OnInit();
}