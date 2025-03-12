using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using R3;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class CreatureBinder : MonoBehaviour, IPointerClickHandler
{
    protected Rigidbody2D rb;
    protected bool movementBlocked = false;

    public CreatureViewModel ViewModel { get; private set; }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ViewModel.Rb = rb;
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        ViewModel.Position.OnNext(rb.position);
    }

    public void Bind(CreatureViewModel viewModel, GoapBehaviour goap)
    {
        ViewModel = viewModel;
        transform.position = ViewModel.Position.Value;
        viewModel.MovementBlocked.Subscribe(b => movementBlocked = b);

        if (viewModel.AgentType != AgentTypes.None)
            InitGoap(viewModel, goap);

        OnBind(viewModel);
    }

    private void InitGoap(CreatureViewModel viewModel, GoapBehaviour goap)
    {
        var navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = navMeshAgent.updateUpAxis = false;
        
        var actionProvider = gameObject.AddComponent<GoapActionProvider>();
        
        var agentBehaviour = gameObject.AddComponent<AgentBehaviour>();
        agentBehaviour.ActionProvider = actionProvider;

        var agentMoveBehaviour = gameObject.AddComponent<AgentMoveBehaviour>();
        agentMoveBehaviour.Init(navMeshAgent, agentBehaviour);
        
        var brain = AddBrain(viewModel.AgentType);
        brain.Init(agentBehaviour, actionProvider, goap);
    }

    private AgentBrain AddBrain(AgentTypes agentType)
    {
        AgentBrain brain = agentType switch
        {
            AgentTypes.PigAgent => gameObject.AddComponent<PigBrain>(),
            _ => gameObject.AddComponent<PigBrain>(),
        };
        return brain;
    }

    protected virtual void OnBind(CreatureViewModel viewModel) { }
    public void OnPointerClick(PointerEventData eventData)
    {
        ViewModel.OnClick(eventData);
    }
}