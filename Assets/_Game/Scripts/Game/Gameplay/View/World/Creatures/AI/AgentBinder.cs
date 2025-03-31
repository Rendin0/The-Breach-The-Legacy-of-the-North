using R3;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine.AI;
using CrashKonijn.Goap.Core;

public class AgentBinder : CreatureBinder
{
    private AgentViewModel _agentViewModel;
    public override CreatureViewModel ViewModel => _agentViewModel;

    protected override void OnBind(CreatureViewModel viewModel)
    {
        _agentViewModel = viewModel as AgentViewModel;
    }

    public void InitGoap(GoapBehaviour goap, AgentBrain brain)
    {
        var navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = navMeshAgent.updateUpAxis = false;
        _agentViewModel.Stats.Speed.Subscribe(s => navMeshAgent.speed = s);

        var actionProvider = gameObject.AddComponent<GoapActionProvider>();

        var agentBehaviour = gameObject.AddComponent<AgentBehaviour>();
        agentBehaviour.ActionProvider = actionProvider;

        var agentMoveBehaviour = gameObject.AddComponent<AgentMoveBehaviour>();
        agentMoveBehaviour.Init(navMeshAgent, agentBehaviour);

        brain.Init(agentBehaviour, actionProvider, goap, _agentViewModel);
    }
}