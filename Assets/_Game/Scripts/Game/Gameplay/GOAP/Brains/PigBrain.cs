using R3;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using UnityEngine.UI;   

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
        creatureSensor.OnCreatureSpotted.Subscribe(c => OnCreatureSpotted(c));
        creatureSensor.OnCreatureLost.Subscribe(c => OnCreatureLost(c));
    }

    private void OnCreatureSpotted(Collider2D collider)
    {
        LayerMask enemies = creatureBinder.ViewModel.Enemies;
        int layer = collider.gameObject.layer;

        // Проверка на наличие слоя в маске слоёв врагов
        // https://discussions.unity.com/t/checking-if-a-layer-is-in-a-layer-mask/860331/2
        if ((enemies & (1 << layer)) != 0)
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
