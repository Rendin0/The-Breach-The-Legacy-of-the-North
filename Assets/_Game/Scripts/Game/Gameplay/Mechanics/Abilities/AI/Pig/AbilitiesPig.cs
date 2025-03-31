
using UnityEngine;

public class AbilitiesPig : Abilities<UtilsAbilitiesPig>
{
    // ѕол€ дл€ хранени€ вспомогательных утилит и сервиса существ
    private static CreaturesSerivce _creaturesSerivce;


    // »нициализаци€ утилит и сервиса существ
    public AbilitiesPig(CreaturesSerivce creatures)
        : base(creatures)
    {
        utils = new (creatures);
    }

    public static void Attack(PigAgentViewModel caster, Vector2 targetPosition, Vector2 size)
    {
        Vector2 direction = (targetPosition - caster.Position.Value).normalized;
        var points = MathUtils.GetRectPoints(size, caster.Position.Value, direction);
        var targets = utils.DamageRectangle(caster, caster.Stats.Damage.Value, points);
        utils.CreateRectParticle(size, points, direction);
    }

}