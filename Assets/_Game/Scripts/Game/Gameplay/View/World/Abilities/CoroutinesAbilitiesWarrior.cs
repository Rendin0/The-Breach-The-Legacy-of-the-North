
using System.Collections;
using System.Collections.Generic;
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
        var (p1, p2) = MathUtils.GetRectPoints(size, caster.Position.Value, direction);

        caster.Rb.linearVelocity = size.y / time * direction;
        caster.MovementBlocked.OnNext(true);

        yield return new WaitForSeconds(time);

        caster.Rb.linearVelocity = Vector2.zero;
        caster.MovementBlocked.OnNext(false);
        var hits = HitRectangle(caster, damageMultiplier, p1, p2);

        foreach (var hit in hits)
        {
            var slow = new SESlow(slowPower);
            var tmpEffect = new TemporaryStatusEffect(hit.ViewModel, slow, slowDuration);

            hit.ViewModel.AddStatusEffect(tmpEffect);
        }
    }

    public List<CreatureBinder> HitRectangle(CreatureViewModel caster, float damageMultiplier, Vector2 p1, Vector2 p2)
    {
        var hits = Physics2D.OverlapAreaAll(p1, p2);
        var hitsResult = new List<CreatureBinder>();
        foreach (var hit in hits)
        {
            // Ударило существо, ударило не само себя
            if (hit.TryGetComponent<CreatureBinder>(out var creature) && creature.ViewModel.CreatureId != caster.CreatureId)
            {
                _creaturesSerivce.DamageCreature(creature.ViewModel, caster.Stats.Damage.Value * damageMultiplier);
                hitsResult.Add(creature);
            }
        }

        return hitsResult;
    }
}