
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CoroutinesAbilitiesWarrior
{
    private CreaturesSerivce _creaturesSerivce;
    public CoroutinesAbilitiesWarrior(CreaturesSerivce creatures)
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
        var hits = DamageRectangle(caster, damageMultiplier, points);

        foreach (var hit in hits)
        {
            var slow = new SESpeedChange(slowPower, true);
            var tmpEffect = new TemporaryStatusEffect(hit.ViewModel, slow, slowDuration);

            hit.ViewModel.AddStatusEffect(tmpEffect);
        }


    }

    public IEnumerator ExecutionersMarkCoroutine(CreatureViewModel caster, CreatureViewModel target, float time)
    {
        target.MarkCount++;
        target.MarkCount = Mathf.Clamp(target.MarkCount, 0, 3);

        if (target.MarkCount >= 3)
            caster.MarkedTargets.Add(target);

        yield return new WaitForSeconds(time);

        target.MarkCount--;
        target.MarkCount = Mathf.Clamp(target.MarkCount, 0, 3);
        if (target.MarkCount == 2)
            caster.MarkedTargets.Remove(target);
    }

    public List<CreatureBinder> DamageRectangle(CreatureViewModel caster, float damageMultiplier, List<Vector2> points)
    {
        var hits = Physics2DUtils.GetColliderHits<CreatureBinder>(points);
        var hitsResult = new List<CreatureBinder>();
        foreach (var hit in hits)
        {
            // Ударило не само себя
            if (hit.ViewModel.CreatureId != caster.CreatureId)
            {
                hit.ViewModel.Damage(caster.Stats.Damage.Value * damageMultiplier);
                hitsResult.Add(hit);
            }
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