using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class AgentMoveBehaviour : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private AgentBehaviour _agentBehaviour;
    private ITarget _currentTarget;

    [SerializeField] private float _minMoveDist = .2f;
    private Vector2 _lastPosition;

    public void Init(NavMeshAgent navMeshAgent, AgentBehaviour agentBehaviour)
    {
        _navMeshAgent = navMeshAgent;
        _agentBehaviour = agentBehaviour;

        _agentBehaviour.Events.OnTargetChanged += EventOnTargetChanged;
        _agentBehaviour.Events.OnTargetNotInRange += EventOnTargetNotInRange;
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

    private void OnDestroy()
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

    private void OnDrawGizmos()
    {
        if (this._currentTarget == null)
            return;

        Gizmos.DrawLine(this.transform.position, this._currentTarget.Position);
    }
}