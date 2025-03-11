
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
    public AgentTypes AgentType;

    public Rigidbody2D Rb { get; set; }
    public ReactiveProperty<Vector2> Position { get; }
    public ReactiveProperty<bool> MovementBlocked { get; } = new(false);

    public CreatureStatsViewModel Stats { get; private set; }

    public readonly Subject<CreatureViewModel> OnCreatureClick = new();
    public readonly Subject<CreatureViewModel> DeleteRequest = new();
    public readonly Subject<CreatureViewModel> KillRequest = new();

    private readonly List<IStatusEffect> _statusEffects = new();

    public readonly float HealthChangesTime = 5f;
    public float HealthChanges { get; private set; }
    public float PhysicalDamageResistance { get; private set; }
    public float MagicalDamageResistance { get; private set; }
    public float AttackSpeed { get; private set; }

    public int MarkCount = 0;
    public bool InfiniteRage = false;
    public List<CreatureViewModel> MarkedTargets { get; } = new();

    protected Ability attack;

    public CreatureViewModel(CreatureEntityProxy creatureEntity)
    {
        _creatureEntity = creatureEntity;

        TypeId = _creatureEntity.TypeId;
        CreatureId = _creatureEntity.Id;
        Position = _creatureEntity.Position;

        _baseStats = creatureEntity.Stats.Copy();
        Stats = new(creatureEntity.Stats);
        Stats.Defense.Subscribe(v => PhysicalDamageResistance = CalculateDamageResistance(v));
        Stats.Resistance.Subscribe(v => MagicalDamageResistance = CalculateDamageResistance(v));
        Stats.AttackSpeed.Subscribe(v => AttackSpeed = CalculateAttackSpeed(v));
    }

    public virtual void OnClick(PointerEventData eventData)
    {
        OnCreatureClick.OnNext(this);
    }

    public virtual void Attack(Vector2 position)
    {
        if (attack.Use(this, position))
            attack.SetCooldown(AttackSpeed);
    }


    // True - ���
    // False - ����
    public bool Damage(DamageData damage)
    {
        bool isAlive = true;

        if (!Stats.Immortal.Value)
        {
            // ���������� �����
            float damageResult = Mathf.Abs(damage.PhysicalData) * (1f - (PhysicalDamageResistance / 100f));

            // ���������� �����
            damageResult += Mathf.Abs(damage.MagicalData) * (1f - (MagicalDamageResistance / 100f));

            // ��� ���������� ����� 
            damageResult *= 1f - Stats.DamageResistance.Value;
            Stats.Health.OnNext(Stats.Health.Value - damageResult);

            // �� ������ ��������, ������� ��������� �������� ���� ��.
            // ����� ����� ������� ��������� ��� �� ������� ��
            _baseStats.Health.OnNext(Stats.Health.Value / Stats.MaxHealth.Value * _baseStats.MaxHealth.Value);

            // ��� ������������ Delayed reckoning, Unbreakable, ������� ���-�� ����� �� ��������� 5 ������
            HealthChanges += damageResult;
            GameEntryPoint.Coroutines.StartCoroutine(HealthChangesTimer(damageResult, HealthChangesTime));
            isAlive = Stats.Health.Value > 0;
        }

        if (!isAlive)
            KillRequest.OnNext(this);

        return isAlive;
    }

    public void Heal(float heal)
    {
        heal = Mathf.Abs(heal);
        Stats.Health.OnNext(Mathf.Clamp(Stats.Health.Value + heal, 0f, Stats.MaxHealth.Value));

        // �� ������ ��������, ������� ��������� �������� ���� ��.
        // ����� ����� ������� ��������� ��� �� ������� ��
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

    // ��������� ����� ������ � ������� ���������� �����
    private float CalculateDamageResistance(float defense)
    {
        // y = 30log_10(0.1x + 1)
        return 30 * Mathf.Log10(0.1f * defense + 1);
    }

    // ��������� ����� �������� ����� � �������� ����� ������� � ��������
    private float CalculateAttackSpeed(float attackSpeed)
    {
        // (log_0.6(0.00009x) / 10) - 0.06
        return 0.1f * Mathf.Log(0.00009f * attackSpeed, 0.6f) - 0.06f;
    }

    private IEnumerator HealthChangesTimer(float amount, float timer)
    {
        yield return new WaitForSeconds(timer);
        HealthChanges -= amount;
    }
}