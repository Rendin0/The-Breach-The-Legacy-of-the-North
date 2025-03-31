
using System.Collections.Generic;
using UnityEngine;

public class UtilsAbilities
{
    // Поля для хранения вспомогательных утилит и сервиса существ
    protected readonly CreaturesSerivce creaturesSerivce;

    // Инициализация утилит и сервиса существ
    public UtilsAbilities(CreaturesSerivce creatures)
    {
        creaturesSerivce = creatures;
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

            creaturesSerivce.DamageCreature(hit.ViewModel, caster, damage);
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

            creaturesSerivce.DamageCreature(hit.ViewModel, caster, damage);
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