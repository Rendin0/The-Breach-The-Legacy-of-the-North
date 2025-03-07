using CrashKonijn.Goap.Runtime;
using UnityEngine;

[RequireComponent(typeof(GoapActionProvider))]
public class AgentTypeBinder : MonoBehaviour
{
    private GoapBehaviour _goapBehaviour;
    private GoapActionProvider _agent;
    private BrainBase _brain;

    private void Start()
    {
        _agent = GetComponent<GoapActionProvider>();
        _agent.AgentType = _goapBehaviour.GetAgentType(_brain.AgentType.ToString());
    }

    public void Init(GoapBehaviour goapBehaviour, BrainBase brain)
    {
        _goapBehaviour = goapBehaviour;
        _brain = brain;
    }
}