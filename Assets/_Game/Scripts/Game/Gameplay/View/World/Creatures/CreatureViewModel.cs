
using R3;
using System.Collections;
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

    public float HealthChanges { get; private set;}

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

        // Для способности Unbreakable, считает кол-во урона за последние 5 секунд
        HealthChanges += damage;
        GameEntryPoint.Coroutines.StartCoroutine(HealthChangesTimer(damage, 5f));

        return Stats.Health.Value > 0;
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
            effect.Apply(Stats);
    }

    private IEnumerator HealthChangesTimer(float amount, float timer)
    {
        yield return new WaitForSeconds(timer);
        HealthChanges -= amount;
    }
}