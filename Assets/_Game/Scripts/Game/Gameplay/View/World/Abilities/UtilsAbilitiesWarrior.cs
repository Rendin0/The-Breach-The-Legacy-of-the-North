
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        if (target.MarkCount == 2)
            caster.MarkedTargets.Add(target);

        target.MarkCount++;
        target.MarkCount = Mathf.Clamp(target.MarkCount, 0, int.MaxValue);

        yield return new WaitForSeconds(time);

        target.MarkCount--;
        target.MarkCount = Mathf.Clamp(target.MarkCount, 0, int.MaxValue);

        if (target.MarkCount == 2)
            caster.MarkedTargets.Remove(target);
    }

    public IEnumerator DelayedReckoningCoroutine(CreatureViewModel caster, float damageResistancePercent, float radius)
    {
        caster.Stats.DamageResistance.OnNext(caster.Stats.DamageResistance.Value + damageResistancePercent);
        yield return new WaitForSeconds(caster.HealthChangesTime);
        caster.Stats.DamageResistance.OnNext(caster.Stats.DamageResistance.Value - damageResistancePercent);

        float delayedDamage = caster.HealthChanges * (1f / (1f - damageResistancePercent)) * damageResistancePercent;

        var damage = new DamageData() { PhysicalData = delayedDamage };

        DamageCircle(caster, damage, caster.Position.Value, radius);
    }

    public List<CreatureBinder> DamageRectangle(CreatureViewModel caster, DamageData damage, List<Vector2> points)
    {
        var hits = Physics2DUtils.GetColliderHits<CreatureBinder>(points);
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
        var hits = Physics2DUtils.GetCircleHits<CreatureBinder>(center, radius);
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