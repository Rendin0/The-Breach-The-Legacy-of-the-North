using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using System;
using UnityEngine;

public abstract class AgentBrain : MonoBehaviour
{
    protected AgentBehaviour agentBehaviour;
    protected GoapActionProvider provider;
    protected GoapBehaviour goap;

    public void Init(AgentBehaviour agentBehaviour, GoapActionProvider goapActionProvider, GoapBehaviour goapBehaviour)
    {
        this.agentBehaviour = agentBehaviour;
        this.provider = goapActionProvider;
        this.goap = goapBehaviour;

        OnInit();
    }

    protected abstract void OnInit();
}