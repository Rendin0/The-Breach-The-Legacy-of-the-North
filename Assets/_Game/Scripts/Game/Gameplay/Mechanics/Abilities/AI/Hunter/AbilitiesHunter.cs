
using UnityEngine;

public class AbilitiesHunter : Abilities<UtilsAbilitiesHunter>
{
    private static CreaturesSerivce _creatures;

    public AbilitiesHunter(CreaturesSerivce creatures) : base(creatures)
    {
        utils = new(creatures);
        _creatures = creatures;
    }

    public static void Attack(HunterAgentViewModel caster, Vector2 targetPosition, Vector2 size)
    {
        Vector2 direction = (targetPosition - caster.Position.Value).normalized;
        var points = MathUtils.GetRectPoints(size, caster.Position.Value, direction);
        var targets = utils.DamageRectangle(caster, caster.Stats.Damage.Value, points);
        utils.CreateRectParticle(size, points, direction);
    }

    public static void Heal(HunterAgentViewModel caster, float healPercent)
    {
        _creatures.HealCreature(caster, caster, caster.Stats.MaxHealth.Value * healPercent);
    }
}