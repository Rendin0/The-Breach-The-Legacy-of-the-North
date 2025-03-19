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

    public void Bind(CreatureViewModel viewModel, GoapBehaviour goap, AgentBrain brain)
    {
        ViewModel = viewModel;
        transform.position = ViewModel.Position.Value;
        viewModel.MovementBlocked.Subscribe(b => movementBlocked = b);

        if (viewModel.AgentType != AgentTypes.None)
            InitGoap(viewModel, goap, brain);

        OnBind(viewModel);
    }

    private void InitGoap(CreatureViewModel viewModel, GoapBehaviour goap, AgentBrain brain)
    {
        var navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = navMeshAgent.updateUpAxis = false;
        viewModel.Stats.Speed.Subscribe(s => navMeshAgent.speed = s);

        var actionProvider = gameObject.AddComponent<GoapActionProvider>();
        
        var agentBehaviour = gameObject.AddComponent<AgentBehaviour>();
        agentBehaviour.ActionProvider = actionProvider;

        var agentMoveBehaviour = gameObject.AddComponent<AgentMoveBehaviour>();
        agentMoveBehaviour.Init(navMeshAgent, agentBehaviour);
        
        brain.Init(agentBehaviour, actionProvider, goap, this);
    }

    protected virtual void OnBind(CreatureViewModel viewModel) { }
    public void OnPointerClick(PointerEventData eventData)
    {
        ViewModel.OnClick(eventData);
    }
}