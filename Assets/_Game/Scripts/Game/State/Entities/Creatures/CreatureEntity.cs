
using System;
using UnityEngine;

[Serializable]
public class CreatureEntity : Entity
{
    public string TypeId;
    public Vector2 Position;
    public CreatureStats Stats;
    public AgentTypes AgentType;
    public Factions Faction;

    public CreatureEntity(CreatureStatsConfig stats)
    {
        Stats = new(stats);
    }
}
