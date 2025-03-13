
using R3;
using UnityEngine;

public class CreatureEntityProxy : EntityProxy
{
    public CreatureEntity Origin { get; }

    public string TypeId => Origin.TypeId;
    public AgentTypes AgentType => Origin.AgentType;
    public ReactiveProperty<Vector2> Position { get; }
    public CreatureStatsProxy Stats { get; }
    public Factions Faction => Origin.Faction;

    public CreatureEntityProxy(CreatureEntity creatureEntity)
        : base(creatureEntity)
    {
        Origin = creatureEntity;
        Stats = new(creatureEntity.Stats);

        Position = new ReactiveProperty<Vector2>(creatureEntity.Position);
        Position.Skip(1).Subscribe(value => creatureEntity.Position = value);
    }
}