
using R3;
using UnityEngine;

public class CreatureEntityProxy : EntityProxy
{
    public CreatureEntity Origin { get; }

    public string TypeId { get; }
    public ReactiveProperty<Vector2> Position { get; }
    public CreatureStatsProxy Stats { get; }


    public CreatureEntityProxy(CreatureEntity creatureEntity)
        : base(creatureEntity)
    {
        Origin = creatureEntity;
        Stats = new(creatureEntity.Stats);

        TypeId = creatureEntity.TypeId;
        Position = new ReactiveProperty<Vector2>(creatureEntity.Position);
        Position.Skip(1).Subscribe(value => creatureEntity.Position = value);
    }
}