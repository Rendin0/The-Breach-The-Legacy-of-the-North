using CrashKonijn.Goap.Runtime;
using UnityEngine;

[RequireComponent(typeof(GoapActionProvider))]
public class AgentTypeBinder : MonoBehaviour
{
    public GoapBehaviour GoapBehaviour;

    private void Start()
    {
        GoapActionProvider agent = GetComponent<GoapActionProvider>();
        agent.AgentType = GoapBehaviour.GetAgentType("Enemy");
    }
}