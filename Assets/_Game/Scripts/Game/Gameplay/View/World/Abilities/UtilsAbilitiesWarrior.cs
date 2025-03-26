using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsAbilitiesWarrior
{
    private CreaturesSerivce _creaturesSerivce;
    public UtilsAbilitiesWarrior(CreaturesSerivce creatures)
    {
        _creaturesSerivce = creatures;
    }

    public IEnumerator DashCoroutine(CreatureViewModel caster, float time, Vector2 size, Vector2 direction, float damageMultiplier, float slowPower, float slowDuration)
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

    public IEnumerator ExecutionersMarkCoroutine(CreatureViewModel caster, CreatureViewModel target, float time)
    {
        if (caster is WarriorViewModel war)
        {
            if (target.DynamicStats.MarkCount == 2)
                war.MarkedTargets.Add(target);

            target.DynamicStats.MarkCount++;
            target.DynamicStats.MarkCount = Mathf.Clamp(target.DynamicStats.MarkCount, 0, int.MaxValue);

            yield return new WaitForSeconds(time);

            target.DynamicStats.MarkCount--;
            target.DynamicStats.MarkCount = Mathf.Clamp(target.DynamicStats.MarkCount, 0, int.MaxValue);

            if (target.DynamicStats.MarkCount == 2)
                war.MarkedTargets.Remove(target);
        }
        yield return null;
    }

    public IEnumerator DelayedReckoningCoroutine(CreatureViewModel caster, float damageResistancePercent, float radius)
    {
        caster.Stats.DamageResistance.OnNext(caster.Stats.DamageResistance.Value + damageResistancePercent);
        yield return new WaitForSeconds(caster.DynamicStats.HealthChangesTime);
        caster.Stats.DamageResistance.OnNext(caster.Stats.DamageResistance.Value - damageResistancePercent);

        float delayedDamage = caster.DynamicStats.HealthChanges * (1f / (1f - damageResistancePercent)) * damageResistancePercent;

        var damage = new DamageData() { PhysicalData = delayedDamage };

        DamageCircle(caster, damage, caster.Position.Value, radius);
    }

    public List<CreatureBinder> DamageRectangle(CreatureViewModel caster, DamageData damage, List<Vector2> points)
    {
        var mask = caster.Enemies;
        var hits = Physics2DUtils.GetColliderHits<CreatureBinder>(points, mask);
        var hitsResult = new List<CreatureBinder>();

        foreach (var hit in hits)
        {
            // Ударило не само себя
            if (hit.ViewModel.CreatureId == caster.CreatureId) continue;

            hit.ViewModel.Damage(damage);
            hitsResult.Add(hit);
        }

        return hitsResult;
    }

    public List<CreatureBinder> DamageCircle(CreatureViewModel caster, DamageData damage, Vector2 center, float radius)
    {
        var mask = caster.Enemies;
        var hits = Physics2DUtils.GetCircleHits<CreatureBinder>(center, radius, mask);
        var hitsResult = new List<CreatureBinder>();
        foreach (var hit in hits)
        {
            // Ударило не само себя
            if (hit.ViewModel.CreatureId == caster.CreatureId) continue;

            hit.ViewModel.Damage(damage);
            hitsResult.Add(hit);
        }

        return hitsResult;
    }

    public void CreateRectParticle(Vector2 size, List<Vector2> points, Vector2 direction)
    {
        var particle = new GameObject("Slash");
        particle.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Gameplay/Particles/WarriorSlash");
        particle.transform.position = Vector2.Lerp(points[0], points[2], .5f);
        particle.transform.localScale = size;
        particle.transform.Rotate(new Vector3(0f, 0f, (-new Vector2(direction.x, 0).normalized.x) * Vector2.Angle(direction, Vector2.up)));

        GameObject.Destroy(particle, .5f);
    }
}