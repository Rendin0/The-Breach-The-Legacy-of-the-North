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

        agent.ThreatMap.ObserveAdd().Subscribe(pair => ThreatAdded(pair.Value.Key, pair.Value.Value));
        agent.ThreatMap.ObserveRemove().Subscribe(pair => ThreatRemoved(pair.Value.Key, pair.Value.Value));
        agent.ThreatMap.ObserveChanged().Subscribe(pair => ThreatChanged(pair.NewItem.Key, pair.NewItem.Value));
    }

    private void ThreatChanged(CreatureViewModel key, float value)
    {
    }

    private void ThreatAdded(CreatureViewModel creature, float threat)
    {
        agentBehaviour.StopAction();
        provider.RequestGoal<KillEnemyGoal>();
    }

    private void ThreatRemoved(CreatureViewModel creature, float threat)
    {
    }

    private void AddCreatureSensor()
    {
        _creatureSensor = gameObject.AddComponent<CreatureSensor>();
        _creatureSensor.OnEnemySpotted.Subscribe(c => OnEnemySpotted(c));
        _creatureSensor.OnCreatureLost.Subscribe(c => OnCreatureLost(c));

    }

    private void OnCreatureLost(Collider2D collider)
    {

    }

    private void OnEnemySpotted(Collider2D collider)
    {
        var threatDealer = collider.GetComponent<CreatureBinder>().ViewModel;
        agent.CreatureRequests.ThreatAddedRequest.OnNext((threatDealer, 10));
    }
}