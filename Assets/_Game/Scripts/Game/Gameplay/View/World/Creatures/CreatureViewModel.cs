
using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatureViewModel : IBuffable
{
    private readonly CreatureEntityProxy _creatureEntity;
    private readonly CreatureStatsProxy _baseStats;
    public CreatureStatsViewModel Stats { get; private set; }
    public DynamicCreatureStats DynamicStats;

    public int CreatureId => _creatureEntity.Id;
    public string TypeId => _creatureEntity.TypeId;
    public AgentTypes AgentType => _creatureEntity.AgentType;

    public Factions Faction => _creatureEntity.Faction;
    private LayerMask _enemies = -2;
    public LayerMask Enemies
    {
        get
        {
            if (_enemies == -2)
                _enemies = FactionManager.GetEnemies(Faction);

            return _enemies;
        }
    }
    public CreatureViewModel CurrentTarget { get; set; }

    public Rigidbody2D Rb { get; set; }
    public ReactiveProperty<Vector2> Position { get; }
    public ReactiveProperty<bool> MovementBlocked { get; } = new(false);

    private readonly List<IStatusEffect> _statusEffects = new();
    public CreatureRequests CreatureRequests = new();

    protected Ability attack;

    public CreatureViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig)
    {
        _creatureEntity = creatureEntity;

        Position = _creatureEntity.Position;

        _baseStats = creatureEntity.Stats.Copy();
        Stats = new(creatureEntity.Stats);
        DynamicStats = new(Stats);

        attack = new(abilitiesConfig.Attack);
    }

    public virtual void OnClick(PointerEventData eventData)
    {
        CreatureRequests.OnCreatureClick.OnNext(this);
    }


    public virtual bool Attack(Vector2 position)
    {
        bool result = attack.Use(this, position);

        if (result)
            attack.SetCooldown(DynamicStats.AttackSpeed);

        return result;
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
            _baseStats.Health.OnNext(Stats.Health.Value / Stats.MaxHealth.Value * _baseStats.MaxHealth.Value);

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
        _baseStats.Health.OnNext(Stats.Health.Value / Stats.MaxHealth.Value * _baseStats.MaxHealth.Value);
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
            effect.Apply(this);
    }
}