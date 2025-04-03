using System.Linq;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
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

        provider.Events.OnGoalStart += OnGoalChange;
        provider.Events.OnGoalCompleted += OnGoalChange;

        OnInit();
    }

    private void OnGoalChange(IGoal goal)
    {
        Debug.Log($"Goal changed: {goal}");

        agentBehaviour.StopAction();
    }

    protected abstract void OnInit();
}