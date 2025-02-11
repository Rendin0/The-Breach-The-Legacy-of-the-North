

using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public static class AbilitiesWarrior
{
    private static CoroutinesAbilitiesWarrior _coroutines;
    public static void Init(CreaturesSerivce creatures)
    {
        _coroutines = new(creatures);
    }

    public static void Attack(CreatureViewModel caster, Vector2 mousePosition, Vector2 size)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;
        var points = MathUtils.GetRectPoints(size, caster.Position.Value, direction);

        var targets = _coroutines.DamageRectangle(caster, 1, points);
        _coroutines.CreateRectParticle(size, points, direction);

        foreach (var target in targets)
        {
            GameEntryPoint.Coroutines.StartCoroutine(_coroutines.ExecutionersMarkCoroutine(caster, target.ViewModel, 5));
        }
    }

    public static void Slash(CreatureViewModel caster, Vector2 mousePosition, Vector2 size, float damageMultiplier)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;
        var points = MathUtils.GetRectPoints(size, caster.Position.Value, direction);

        _coroutines.DamageRectangle(caster, damageMultiplier, points);
        _coroutines.CreateRectParticle(size, points, direction);
    }

    public static void Dash(CreatureViewModel caster, Vector2 mousePosition, Vector2 size, float time, float damageMultiplier, float slowPower, float slowDuration)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;

        GameEntryPoint.Coroutines.StartCoroutine(_coroutines.DashCoroutine(caster, time, size, direction, damageMultiplier, slowPower, slowDuration));
    }

    public static void Unbreakable(CreatureViewModel caster, float healPercent, float immortalityDuration, float stunRadius, float stunDuration)
    {
        caster.Heal(healPercent * caster.Stats.MaxHealth.Value);
        var immortality = new SEImmortality();
        var tmpEffect = new TemporaryStatusEffect(caster, immortality, immortalityDuration);
        caster.AddStatusEffect(tmpEffect);

        // Стан всех врагов в радиусе
        var hits = Physics2DUtils.GetCircleHits<CreatureBinder>(caster.Position.Value, stunRadius);
        tmpEffect = new TemporaryStatusEffect(caster, new SEStun(), stunDuration);

        foreach (var hit in hits)
        {
            hit.ViewModel.AddStatusEffect(tmpEffect);
        }
    }

    public static void EnduringPower(CreatureViewModel caster, float damagePercent, float resistance, float defense, float duration)
    {
        var dmgUp = new SEDamageChange(damagePercent, true);
        var tmpEffect = new TemporaryStatusEffect(caster, dmgUp, duration);
        caster.AddStatusEffect(tmpEffect);

        var defUp = new SEDefenseChange(defense);
        tmpEffect = new TemporaryStatusEffect(caster, defUp, duration);
        caster.AddStatusEffect(tmpEffect);

        // TODO +resistance

    }

    public static void ExecutionersMark(CreatureViewModel caster, float totalDamage, float duration)
    {
        foreach (var target in caster.MarkedTargets)
        {
            target.MarkCount = 0;
            target.AddStatusEffect(new SEDot(target, totalDamage, duration));
        }
    }

    public static void EarthRift(CreatureViewModel caster, Vector2 mousePosition, Vector2 size, float debuffDuration, float damageAmplify)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;
        var stunEffect = new TemporaryStatusEffect(caster, new SEStun(), debuffDuration);
        var amplifyEffect = new TemporaryStatusEffect(caster, new SEDamageChange(-damageAmplify, true), debuffDuration);

        var points = MathUtils.GetRectPoints(size, caster.Position.Value, direction);
        var hits = Physics2DUtils.GetColliderHits<CreatureBinder>(points);

        _coroutines.CreateRectParticle(size, points, direction);

        foreach (var hit in hits )
        {
            if (hit.ViewModel.CreatureId == caster.CreatureId) continue;

            hit.ViewModel.AddStatusEffect(stunEffect);
            hit.ViewModel.AddStatusEffect(amplifyEffect);
        }

    }
}