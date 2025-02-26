
using R3;
using UnityEngine;

public class CreatureEntityProxy : EntityProxy, IDamageable
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

    // True - alive
    // False - dead
    public bool Damage(float damage)
    {
        damage = Mathf.Abs(damage);
        Stats.Health.OnNext(Stats.Health.Value - damage);

        return Stats.Health.Value > 0;
    }
}