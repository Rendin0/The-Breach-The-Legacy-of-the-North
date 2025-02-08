
using R3;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatureViewModel : IBuffable
{
    private readonly CreatureEntityProxy _creatureEntity;
    private readonly CreatureStatsProxy _baseStats;

    public readonly int CreatureId;
    public readonly string TypeId;

    public Rigidbody2D Rb { get; set; }
    public ReactiveProperty<Vector2> Position { get; }
    public ReactiveProperty<bool> MovementBlocked { get; } = new(false);

    public CreatureStatsViewModel Stats {get; private set;}

    public readonly Subject<CreatureViewModel> OnCreatureClick = new();
    public readonly Subject<CreatureViewModel> DeleteRequest = new();

    private List<IStatusEffect> _statusEffects = new();

    public CreatureViewModel(CreatureEntityProxy creatureEntity)
    {
        _creatureEntity = creatureEntity;

        TypeId = _creatureEntity.TypeId;
        CreatureId = _creatureEntity.Id;
        Position = _creatureEntity.Position;

        _baseStats = creatureEntity.Stats.Copy();
        Stats = new(creatureEntity.Stats);
    }

    public virtual void OnClick(PointerEventData eventData)
    {
        OnCreatureClick.OnNext(this);
    }


    // True - жив
    // False - мёртв
    public bool Damage(float damage)
    {
        damage = Mathf.Abs(damage);
        Stats.Health.OnNext(Stats.Health.Value - damage);

        // На случай дебаффов, которые уменьшают временно макс хп.
        // После конца дебаффа останется тот же процент хп
        _baseStats.Health.OnNext(Stats.Health.Value / Stats.MaxHealth.Value * _baseStats.MaxHealth.Value);

        return Stats.Health.Value > 0;
    }

    public void AddStatusEffect(IStatusEffect effect)
    {
        _statusEffects.Add(effect);
        UpdateStatusEffects(_statusEffects);
    }

    public void RemoveStatusEffect(IStatusEffect effect)
    {
        _statusEffects.Remove(effect);
        UpdateStatusEffects(_statusEffects);
    }

    private void UpdateStatusEffects(List<IStatusEffect> effects)
    {
        Stats.CopyFrom(_baseStats);

        foreach (var effect in effects)
            effect.Apply(Stats);
    }
}