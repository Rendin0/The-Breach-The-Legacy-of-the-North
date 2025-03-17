using R3;
using UnityEngine;

public class PigBrain : AgentBrain
{
    public override string AgentType => AgentTypes.PigAgent.ToString();

    private Collider2D _currentTarget = null;

    protected override void OnInit()
    {
        provider.AgentType = goap.GetAgentType(AgentType);

        provider.RequestGoal<IdleGoal>();
        AddCreatureSensor();
    }

    private void AddCreatureSensor()
    {
        var creatureSensor = gameObject.AddComponent<CreatureSensor>();
        creatureSensor.Init(GetComponent<CreatureBinder>());
        creatureSensor.OnEnemySpotted.Subscribe(c => OnEnemySpotted(c));
        creatureSensor.OnCreatureLost.Subscribe(c => OnCreatureLost(c));
    }

    private void OnEnemySpotted(Collider2D collider)
    {
        if (_currentTarget == null)
        {
            agentBehaviour.StopAction();
            provider.RequestGoal<KillEnemyGoal>();
            _currentTarget = collider;
        }
    }

    private void OnCreatureLost(Collider2D collider)
    {
        if (_currentTarget == collider)
        {
            _currentTarget = null;
            provider.RequestGoal<IdleGoal>();
        }
    }
}
