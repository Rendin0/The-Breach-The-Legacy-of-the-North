using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public abstract class AgentBrain : MonoBehaviour
{
    protected AgentBehaviour agentBehaviour;
    protected GoapActionProvider provider;
    protected GoapBehaviour goap;

    public abstract string AgentType { get; }
    public void Init(AgentBehaviour agentBehaviour, GoapActionProvider goapActionProvider, GoapBehaviour goapBehaviour)
    {
        this.agentBehaviour = agentBehaviour;
        this.provider = goapActionProvider;
        this.goap = goapBehaviour;

        provider.AgentType = goap.GetAgentType(AgentType);

        OnInit();
    }

    protected abstract void OnInit();
}