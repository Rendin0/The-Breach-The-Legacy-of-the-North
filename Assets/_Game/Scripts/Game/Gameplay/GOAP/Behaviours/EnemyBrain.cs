
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

[RequireComponent (typeof(AgentBehaviour), typeof(GoapActionProvider))]
public class EnemyBrain : MonoBehaviour
{
    private AgentBehaviour _agentBehaviour;
    private GoapActionProvider _goapActionProvider;

    private void Awake()
    {
        _agentBehaviour = GetComponent<AgentBehaviour>();
        _goapActionProvider = GetComponent<GoapActionProvider>();
    }

    private void Start()
    {
        _goapActionProvider.RequestGoal<WanderGoal>(false);
    }
}