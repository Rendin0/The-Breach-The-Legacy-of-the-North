using CrashKonijn.Agent.Runtime;
using R3;
using System;
using UnityEditor.VersionControl;
using UnityEngine;
using ObservableCollections;

public class AggressiveBrain : AgentBrain
{
    public override string AgentType => AgentTypes.AggressiveAgent.ToString();

    private CreatureSensor _creatureSensor;

    protected override void OnInit()
    {
        AddCreatureSensor();

        provider.RequestGoal<IdleGoal>();

        AddGoalListeners();
    }

    #region Listeners
    private void AddGoalListeners()
    {
        agent.ThreatMap.ObserveAdd().Subscribe(_ => ResolveCurrentGoal());
        agent.ThreatMap.ObserveRemove().Subscribe(_ => ResolveCurrentGoal());
        agent.Stats.Health.Subscribe(_ => ResolveCurrentGoal());
    }

    private void AddCreatureSensor()
    {
        _creatureSensor = gameObject.AddComponent<CreatureSensor>();
        _creatureSensor.OnEnemySpotted.Subscribe(c => OnEnemySpotted(c));
        _creatureSensor.OnEnemyLost.Subscribe(c => OnEnemyLost(c));
    }
    #endregion

    #region Events
    private void OnEnemyLost(Collider2D collider)
    {
        var threatDealer = collider.GetComponent<CreatureBinder>().ViewModel;

        agent.StartRemoveThreatCoroutine(threatDealer);
    }

    private void OnEnemySpotted(Collider2D collider)
    {
        var threatDealer = collider.GetComponent<CreatureBinder>().ViewModel;

        agent.AbortRemoveThreatCoroutine(threatDealer);
        agent.CreatureRequests.ThreatAddedRequest.OnNext((threatDealer, 10));
    }
    #endregion


    protected void ResolveCurrentGoal()
    {
        agentBehaviour.StopAction();

        var healthPercent = agent.Stats.Health.Value / agent.Stats.MaxHealth.Value;

        if (healthPercent <= .2f)
        {
            provider.RequestGoal<SustainSelfGoal>();
            return;
        }

        if (agent.ThreatMap.Count == 0)
        {
            provider.RequestGoal<IdleGoal>();
            return;
        }

        provider.RequestGoal<KillEnemiesGoal>();
    }
}