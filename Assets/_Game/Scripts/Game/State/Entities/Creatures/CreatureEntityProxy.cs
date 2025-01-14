
using R3;
using UnityEngine;

public class CreatureEntityProxy : EntityProxy
{
    public CreatureEntity Origin { get; }

    public string TypeId { get; }
    public ReactiveProperty<Vector2> Position { get; }
    public ReactiveProperty<float> Health { get; }
    public ReactiveProperty<float> MaxHealth { get; }

    public CreatureEntityProxy(CreatureEntity creatureEntity)
        : base(creatureEntity)
    {
        Origin = creatureEntity;

        TypeId = creatureEntity.TypeId;
        Position = new ReactiveProperty<Vector2>(creatureEntity.Position);
        Health = new ReactiveProperty<float>(creatureEntity.Health);
        MaxHealth = new ReactiveProperty<float>(creatureEntity.MaxHealth);

        Position.Skip(1).Subscribe(value => creatureEntity.Position = value);
        Health.Skip(1).Subscribe(value => creatureEntity.Health = value);
        MaxHealth.Skip(1).Subscribe(value => creatureEntity.MaxHealth = value);
    }
}