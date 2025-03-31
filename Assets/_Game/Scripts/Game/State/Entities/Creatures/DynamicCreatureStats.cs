using System.Collections;
using UnityEngine;
using R3;

public class DynamicCreatureStats
{

    public readonly float HealthChangesTime = 5f;
    public float AttackSpeed { get; private set; }
    public float MagicalDamageResistance { get; private set; }
    public float PhysicalDamageResistance { get; private set; }
    public int MarkCount { get; set; } = 0;
    public float HealthChanges { get; set; }

    public DynamicCreatureStats(CreatureStatsViewModel stats)
    {
        stats.Defense.Subscribe(v => PhysicalDamageResistance = CalculateDamageResistance(v));
        stats.Resistance.Subscribe(v => MagicalDamageResistance = CalculateDamageResistance(v));
        stats.AttackSpeed.Subscribe(v => AttackSpeed = CalculateAttackSpeed(v));
    }

    // Переводит число защиты в процент уменьшения урона
    private float CalculateDamageResistance(float defense)
    {
        // y = 30log_10(0.1x + 1)
        return 30 * Mathf.Log10(0.1f * defense + 1);
    }

    // Переводит число скорости атаки в задержку между атаками в секундах
    private float CalculateAttackSpeed(float attackSpeed)
    {
        // (log_0.6(0.00009x) / 10) - 0.06
        return 0.1f * Mathf.Log(0.00009f * attackSpeed, 0.6f) - 0.06f;
    }

    public void HealthChangesTimer(float amount)
    {
        GameEntryPoint.Coroutines.StartCoroutine(HealthChangesCoroutine(amount, HealthChangesTime));
    }

    private IEnumerator HealthChangesCoroutine(float amount, float timer)
    {
        yield return new WaitForSeconds(timer);
        HealthChanges -= amount;
    }
}