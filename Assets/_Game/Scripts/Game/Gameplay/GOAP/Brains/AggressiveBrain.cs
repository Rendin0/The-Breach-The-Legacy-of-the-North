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

        agent.ThreatMap.ObserveAdd().Subscribe(_ => ResolveCurrentGoal());
        agent.ThreatMap.ObserveRemove().Subscribe(_ => ResolveCurrentGoal());
    }
    private void AddCreatureSensor()
    {
        _creatureSensor = gameObject.AddComponent<CreatureSensor>();
        _creatureSensor.OnEnemySpotted.Subscribe(c => OnEnemySpotted(c));
    }

    private void OnEnemySpotted(Collider2D collider)
    {
        var threatDealer = collider.GetComponent<CreatureBinder>().ViewModel;
        agent.CreatureRequests.ThreatAddedRequest.OnNext((threatDealer, 10));
    }
}