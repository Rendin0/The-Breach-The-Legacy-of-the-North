
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentBehaviour), typeof(GoapActionProvider))]
public class AgentMoveBehaviour : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private AgentBehaviour _agentBehaviour;
    private GoapActionProvider _actionProvider;
    private ITarget _currentTarget;

    [SerializeField] private float _minMoveDist = .2f;
    private Vector2 _lastPosition;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = _navMeshAgent.updateUpAxis = false;

        _agentBehaviour = GetComponent<AgentBehaviour>();
        _actionProvider = GetComponent<GoapActionProvider>();
    }

    private void Update()
    {
        if (_currentTarget == null)
            return;

        if (Vector2.Distance(_currentTarget.Position, _lastPosition) >= _minMoveDist)
        {
            _lastPosition = _currentTarget.Position;
            _navMeshAgent.SetDestination(_currentTarget.Position);
        }
    }

    private void OnEnable()
    {
        _agentBehaviour.Events.OnTargetChanged += EventOnTargetChanged;
        _agentBehaviour.Events.OnTargetNotInRange += EventOnTargetNotInRange;
    }

    private void OnDisable()
    {
        _agentBehaviour.Events.OnTargetChanged -= EventOnTargetChanged;
        _agentBehaviour.Events.OnTargetNotInRange -= EventOnTargetNotInRange;
    }

    private void EventOnTargetNotInRange(ITarget target)
    {
        // Animator
    }

    private void EventOnTargetChanged(ITarget target, bool inRange)
    {
        _currentTarget = target;
        _lastPosition = _currentTarget.Position;
        _navMeshAgent.SetDestination(_currentTarget.Position);
        // Animator
    }
}