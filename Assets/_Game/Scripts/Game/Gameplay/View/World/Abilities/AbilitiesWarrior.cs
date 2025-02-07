

using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public static class AbilitiesWarrior
{
    public static void Slash(CreatureViewModel caster, Vector2 mousePosition)
    {
        Debug.Log($"{caster} + {mousePosition}");

    }

    public static void Dash(CreatureViewModel caster, Vector2 mousePosition, Vector2 size, float time, float damage)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;

        GameEntryPoint.Coroutines.StartCoroutine(DashCoroutine(caster, time, size, direction, damage));
    }

    private static IEnumerator DashCoroutine(CreatureViewModel caster, float time, Vector2 size, Vector2 direction, float damage)
    {
        var (p1, p2) = MathUtils.GetRectPoints(size, caster.Position.Value, direction);

        caster.Rb.linearVelocity = size.y / time * direction;
        caster.MovementBlocked.OnNext(true);

        yield return new WaitForSeconds(time);

        caster.Rb.linearVelocity = Vector2.zero;
        caster.MovementBlocked.OnNext(false);

        var hits = Physics2D.OverlapAreaAll(p1, p2);
        foreach (var hit in hits)
        {
            // Ударило существо, ударило не само себя
            if (hit.TryGetComponent<CreatureBinder>(out var creature) && creature.ViewModel.CreatureId != caster.CreatureId)
            {
                Debug.Log($"Hitted {creature.name}");
            }
        }
    }
}