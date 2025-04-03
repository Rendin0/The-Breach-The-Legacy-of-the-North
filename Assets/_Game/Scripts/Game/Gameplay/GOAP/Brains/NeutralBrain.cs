using System;
using ObservableCollections;
using R3;
using UnityEngine;

public class NeutralBrain : AgentBrain
{
    private CreatureSensor _creatureSensor;

    public override string AgentType => AgentTypes.NeutralAgent.ToString();

    protected override void OnInit()
    {
        provider.RequestGoal<KillEnemiesGoal, IdleGoal>();
        //agent.ThreatMap.ObserveAdd().Subscribe(_ => ResolveCurrentGoal());
        //agent.ThreatMap.ObserveRemove().Subscribe(_ => ResolveCurrentGoal());
        //agent.Stats.Health.Subscribe(_ => ResolveCurrentGoal());

        AddCreatureSensor();
    }

    private void AddCreatureSensor()
    {
        _creatureSensor = gameObject.AddComponent<CreatureSensor>();
        _creatureSensor.OnEnemySpotted.Subscribe(c => OnEnemySpotted(c));
        _creatureSensor.OnEnemyLost.Subscribe(c => OnEnemyLost(c));
    }

    private void OnEnemySpotted(Collider2D collider)
    {
        var threatDealer = collider.GetComponent<CreatureBinder>().ViewModel;

        agent.AbortRemoveThreatCoroutine(threatDealer);
    }

    private void OnEnemyLost(Collider2D collider)
    {
        var threatDealer = collider.GetComponent<CreatureBinder>().ViewModel;

        agent.StartRemoveThreatCoroutine(threatDealer);
    }

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