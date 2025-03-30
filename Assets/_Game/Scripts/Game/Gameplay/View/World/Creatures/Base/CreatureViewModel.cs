
using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CreatureViewModel : IBuffable
{
    protected readonly CreatureEntityProxy creatureEntity;
    protected readonly CreatureStatsProxy baseStats;
    public CreatureStatsViewModel Stats { get; private set; }
    public DynamicCreatureStats DynamicStats;

    public int CreatureId => creatureEntity.Id;
    public string TypeId => creatureEntity.TypeId;
    public Factions Faction => creatureEntity.Faction;
    public LayerMask Enemies { get; }

    public ReactiveProperty<bool> MovementBlocked { get; } = new(false);
    public Rigidbody2D Rb { get; set; }
    public ReactiveProperty<Vector2> Position { get; }

    private readonly List<IStatusEffect> _statusEffects = new();
    public CreatureRequests CreatureRequests = new();


    public CreatureViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig)
    {
        this.creatureEntity = creatureEntity;

        Position = this.creatureEntity.Position;

        baseStats = creatureEntity.Stats.Copy();
        Stats = new(creatureEntity.Stats);
        DynamicStats = new(Stats);

        Enemies = FactionManager.GetEnemies(Faction);
    }

    public virtual void OnClick(PointerEventData eventData)
    {
        CreatureRequests.OnCreatureClick.OnNext(this);
    }

    // True - жив
    // False - мёртв
    public void Damage(DamageData damage)
    {
        bool isAlive = true;

        if (!Stats.Immortal.Value)
        {
            // Физическая часть
            float damageResult = Mathf.Abs(damage.PhysicalData) * (1f - (DynamicStats.PhysicalDamageResistance / 100f));

            // Магическая часть
            damageResult += Mathf.Abs(damage.MagicalData) * (1f - (DynamicStats.MagicalDamageResistance / 100f));

            // Доп уменьшение урона 
            damageResult *= 1f - Stats.DamageResistance.Value;
            Stats.Health.OnNext(Stats.Health.Value - damageResult);

            // На случай дебаффов, которые уменьшают временно макс хп.
            // После конца дебаффа останется тот же процент хп
            baseStats.Health.OnNext(Stats.Health.Value / Stats.MaxHealth.Value * baseStats.MaxHealth.Value);

            // Для способностей Delayed reckoning, Unbreakable, считает кол-во урона за последние 5 секунд
            DynamicStats.HealthChanges += damageResult;
            DynamicStats.HealthChangesTimer(damageResult);
            isAlive = Stats.Health.Value > 0;
        }

        if (!isAlive)
            CreatureRequests.KillRequest.OnNext(this);
    }

    public void Heal(float heal)
    {
        heal = Mathf.Abs(heal);
        Stats.Health.OnNext(Mathf.Clamp(Stats.Health.Value + heal, 0f, Stats.MaxHealth.Value));

        // На случай дебаффов, которые уменьшают временно макс хп.
        // После конца дебаффа останется тот же процент хп
        baseStats.Health.OnNext(Stats.Health.Value / Stats.MaxHealth.Value * baseStats.MaxHealth.Value);
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
        Stats.CopyFrom(baseStats);

        foreach (var effect in effects)
            effect.Apply(this);
    }
}