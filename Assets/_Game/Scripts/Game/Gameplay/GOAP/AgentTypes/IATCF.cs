
using UnityEngine;

public interface IATCF
{
    public AgentBrain GetBrain(GameObject go);
    public string AgentType { get; }
}