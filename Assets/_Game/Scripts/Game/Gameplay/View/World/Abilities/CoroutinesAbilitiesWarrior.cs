
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
        var (p1, p2) = MathUtils.GetRectPoints(size, caster.Position.Value, direction);

        caster.Rb.linearVelocity = size.y / time * direction;
        caster.MovementBlocked.OnNext(true);

        CreateRectParticle(size, p1, p2, direction);

        yield return new WaitForSeconds(time);

        caster.Rb.linearVelocity = Vector2.zero;
        caster.MovementBlocked.OnNext(false);
        var hits = DamageRectangle(caster, damageMultiplier, p1, p2);

        foreach (var hit in hits)
        {
            var slow = new SESpeedChange(slowPower, true);
            var tmpEffect = new TemporaryStatusEffect(hit.ViewModel, slow, slowDuration);

            hit.ViewModel.AddStatusEffect(tmpEffect);
        }


    }

    public List<CreatureBinder> DamageRectangle(CreatureViewModel caster, float damageMultiplier, Vector2 p1, Vector2 p2)
    {
        var hitsResult = Physics2DUtils.GetRectHits<CreatureBinder>(p1, p2);
        foreach (var hit in hitsResult)
        {
            // Ударило не само себя
            if (hit.ViewModel.CreatureId != caster.CreatureId)
            {
                _creaturesSerivce.DamageCreature(hit.ViewModel, caster.Stats.Damage.Value * damageMultiplier);
            }
        }

        return hitsResult;
    }

    public void CreateRectParticle(Vector2 size, Vector2 p1, Vector2 p2, Vector2 direction)
    {
        var particle = new GameObject("Slash");
        particle.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Gameplay/Particles/WarriorSlash");
        particle.transform.position = Vector2.Lerp(p1, p2, .5f);
        particle.transform.localScale = size;
        particle.transform.Rotate(new Vector3(0f, 0f, (-new Vector2(direction.x, 0).normalized.x) * Vector2.Angle(direction, Vector2.up)));
        GameObject.Destroy(particle, .5f);
    }
}