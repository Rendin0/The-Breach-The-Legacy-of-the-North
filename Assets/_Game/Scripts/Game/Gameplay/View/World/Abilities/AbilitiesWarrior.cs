

using System.Collections;
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

        _coroutines.HitRectangle(caster, damageMultiplier, p1, p2);
    }

    public static void Dash(CreatureViewModel caster, Vector2 mousePosition, Vector2 size, float time, float damageMultiplier, float slowPower, float slowDuration)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;

        GameEntryPoint.Coroutines.StartCoroutine(_coroutines.DashCoroutine(caster, time, size, direction, damageMultiplier, slowPower, slowDuration));
    }
}