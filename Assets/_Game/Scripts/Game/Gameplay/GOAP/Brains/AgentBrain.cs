using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using System;
using UnityEngine;

public abstract class AgentBrain : MonoBehaviour
{
    protected AgentBehaviour agentBehaviour;
    protected GoapActionProvider provider;
    protected GoapBehaviour goap;
    protected CreatureBinder creatureBinder;

    public abstract string AgentType { get; }
    public void Init(AgentBehaviour agentBehaviour, GoapActionProvider goapActionProvider, GoapBehaviour goapBehaviour, CreatureBinder creatureBinder)
    {
        this.agentBehaviour = agentBehaviour;
        this.provider = goapActionProvider;
        this.goap = goapBehaviour;
        this.creatureBinder = creatureBinder;

        OnInit();
    }

    protected abstract void OnInit();
}