
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CreatureViewModel : IBuffable, IDisposable
{
    protected readonly CreatureEntityProxy creatureEntity;

    public readonly CreatureStatsProxy BaseStats;
    public CreatureStatsViewModel Stats { get; private set; }
    public DynamicCreatureStats DynamicStats;

    public int CreatureId => creatureEntity.Id;
    public string TypeId => creatureEntity.TypeId;
    public Factions Faction => creatureEntity.Faction;
    public LayerMask Enemies { get; }

    public ReactiveProperty<bool> MovementBlocked { get; } = new(false);
    public Rigidbody2D Rb { get; set; }
    public ReactiveProperty<Vector2> Position { get; }
    public Transform Transform { get; set; }

    private readonly List<IStatusEffect> _statusEffects = new();
    public CreatureRequests CreatureRequests = new();


    public CreatureViewModel(CreatureEntityProxy creatureEntity)
    {
        this.creatureEntity = creatureEntity;

        Position = this.creatureEntity.Position;

        BaseStats = creatureEntity.Stats.Copy();
        Stats = new(creatureEntity.Stats);
        DynamicStats = new(Stats);

        Enemies = FactionManager.GetEnemies(Faction);
    }

    public virtual void OnClick(PointerEventData eventData)
    {
        CreatureRequests.OnCreatureClick.OnNext(this);
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
        Stats.CopyFrom(BaseStats);

        foreach (var effect in effects)
            effect.Apply(this);
    }

    public virtual void Dispose() { }
}