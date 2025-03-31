using System.Collections;
using UnityEngine;

public class UtilsAbilitiesWarrior : UtilsAbilities
{
    public UtilsAbilitiesWarrior(CreaturesSerivce creatures)
        : base(creatures)
    {
    }

    public IEnumerator DashCoroutine(WarriorViewModel caster, float time, Vector2 size, Vector2 direction, float damageMultiplier, float slowPower, float slowDuration)
    {
        var points = MathUtils.GetRectPoints(size, caster.Position.Value, direction);

        caster.Rb.linearVelocity = size.y / time * direction;
        caster.MovementBlocked.OnNext(true);

        CreateRectParticle(size, points, direction);

        yield return new WaitForSeconds(time);

        caster.Rb.linearVelocity = Vector2.zero;
        caster.MovementBlocked.OnNext(false);
        var hits = DamageRectangle(caster, caster.Stats.Damage * damageMultiplier, points);

        foreach (var hit in hits)
        {
            var slow = new SESpeedChange(slowPower, true);
            var tmpEffect = new TemporaryStatusEffect(hit.ViewModel, slow, slowDuration);

            hit.ViewModel.AddStatusEffect(tmpEffect);
        }


    }

    public IEnumerator ExecutionersMarkCoroutine(WarriorViewModel caster, CreatureViewModel target, float time)
    {
        if (target.DynamicStats.MarkCount == 2)
            caster.MarkedTargets.Add(target);

        target.DynamicStats.MarkCount++;
        target.DynamicStats.MarkCount = Mathf.Clamp(target.DynamicStats.MarkCount, 0, int.MaxValue);

        yield return new WaitForSeconds(time);

        target.DynamicStats.MarkCount--;
        target.DynamicStats.MarkCount = Mathf.Clamp(target.DynamicStats.MarkCount, 0, int.MaxValue);

        if (target.DynamicStats.MarkCount == 2)
            caster.MarkedTargets.Remove(target);

        yield return null;
    }

    public IEnumerator DelayedReckoningCoroutine(WarriorViewModel caster, float damageResistancePercent, float radius)
    {
        caster.Stats.DamageResistance.OnNext(caster.Stats.DamageResistance.Value + damageResistancePercent);
        yield return new WaitForSeconds(caster.DynamicStats.HealthChangesTime);
        caster.Stats.DamageResistance.OnNext(caster.Stats.DamageResistance.Value - damageResistancePercent);

        float delayedDamage = caster.DynamicStats.HealthChanges * (1f / (1f - damageResistancePercent)) * damageResistancePercent;

        var damage = new DamageData() { PhysicalData = delayedDamage };

        DamageCircle(caster, damage, caster.Position.Value, radius);
    }

}