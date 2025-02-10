

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

    public static void Slash(CreatureViewModel caster, Vector2 mousePosition, Vector2 size, float damageMultiplier)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;
        var (p1, p2) = MathUtils.GetRectPoints(size, caster.Position.Value, direction);

        var particle = new GameObject("Slash");
        particle.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Gameplay/Particles/WarriorSlash");
        particle.transform.position = Vector2.Lerp(p1, p2, .5f);
        particle.transform.localScale = size;
        particle.transform.Rotate(new Vector3(0f, 0f, (-new Vector2(direction.x, 0).normalized.x) * Vector2.Angle(direction, Vector2.up)));
        GameObject.Destroy(particle, .5f);

        _coroutines.DamageRectangle(caster, damageMultiplier, p1, p2);
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

    public static void EarthRift(CreatureViewModel caster, Vector2 mousePosition, Vector2 size, float debuffDuration, float damageAmplify)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;
        var stunEffect = new TemporaryStatusEffect(caster, new SEStun(), debuffDuration);
        var amplifyEffect = new TemporaryStatusEffect(caster, new SEDamageChange(-damageAmplify, true), debuffDuration);

        var (p1, p2) = MathUtils.GetRectPoints(size, caster.Position.Value, direction);
        var hits = Physics2DUtils.GetRectHits<CreatureBinder>(p1, p2);

        foreach (var hit in hits )
        {
            hit.ViewModel.AddStatusEffect(stunEffect);
            hit.ViewModel.AddStatusEffect(amplifyEffect);
        }

    }
}