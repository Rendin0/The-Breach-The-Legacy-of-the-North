using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

public abstract class ATCF<T> : AgentTypeFactoryBase, IATCF where T : AgentBrain
{
    private string _agentType = null;
    public string AgentType
    {
        get
        {
            if (_agentType == null)
            {
                var brain = gameObject.AddComponent<T>();
                _agentType = brain.AgentType;
                Destroy(brain);
            }

            return _agentType;
        }
    }

    // Do not override Create()
    public override IAgentTypeConfig Create()
    {
        var builder = new AgentTypeBuilder(AgentType);

        OnCreate(builder);

        return builder.Build();
    }

    // Need to override
    public abstract void OnCreate(AgentTypeBuilder builder);

    public AgentBrain GetBrain(GameObject go)
    {
        return go.AddComponent<T>();
    }
}
