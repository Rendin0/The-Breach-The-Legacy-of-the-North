

using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public static class AbilitiesWarrior
{
    public static void Slash(CreatureViewModel caster, Vector2 mousePosition)
    {
        Debug.Log($"{caster} + {mousePosition}");

    }

    public static void Dash(CreatureViewModel caster, Vector2 mousePosition, Vector2 size)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;
        var (p1, p2) = MathUtils.GetRectPoints(size, caster.Position.Value, direction);

        var hits = Physics2D.OverlapAreaAll(p1, p2);
        foreach (var hit in hits)
        {
            Debug.DrawLine(caster.Position.Value, hit.transform.position);
        }
    }
}