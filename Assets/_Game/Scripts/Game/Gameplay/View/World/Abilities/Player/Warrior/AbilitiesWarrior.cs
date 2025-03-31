using UnityEngine;

public class AbilitiesWarrior : Abilities<UtilsAbilitiesWarrior>
{
    public AbilitiesWarrior(CreaturesSerivce creatures) : base(creatures)
    {
        utils = new(creatures);
    }


    public static void Heal(WarriorViewModel caster, float healPercent)
    {
        creaturesSerivce.HealCreature(caster, caster, healPercent * caster.Stats.MaxHealth.Value);
    }

    // јтака: вычисление направлени€, получение точек пр€моугольника, нанесение урона и создание частицы
    public static void Attack(WarriorViewModel caster, Vector2 mousePosition, Vector2 size)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;
        var points = MathUtils.GetRectPoints(size, caster.Position.Value, direction);

        var targets = utils.DamageRectangle(caster, caster.Stats.Damage.Value, points);
        utils.CreateRectParticle(size, points, direction);

        // «апуск корутины дл€ метки палача на каждой цели
        foreach (var target in targets)
        {
            GameEntryPoint.Coroutines.StartCoroutine(utils.ExecutionersMarkCoroutine(caster, target.ViewModel, 5));
        }
    }

    // ”дар: вычисление направлени€, получение точек пр€моугольника, нанесение урона с множителем и создание частицы
    public static void Slash(WarriorViewModel caster, Vector2 mousePosition, Vector2 size, float damageMultiplier)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;
        var points = MathUtils.GetRectPoints(size, caster.Position.Value, direction);

        utils.DamageRectangle(caster, caster.Stats.Damage * damageMultiplier, points);
        utils.CreateRectParticle(size, points, direction);
    }

    // –ывок: вычисление направлени€ и запуск корутины дл€ выполнени€ рывка
    public static void Dash(WarriorViewModel caster, Vector2 mousePosition, Vector2 size, float time, float damageMultiplier, float slowPower, float slowDuration)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;

        GameEntryPoint.Coroutines.StartCoroutine(utils.DashCoroutine(caster, time, size, direction, damageMultiplier, slowPower, slowDuration));
    }

    // Ќесокрушимость: лечение, добавление эффекта бессмерти€ и оглушение врагов в радиусе
    public static void Unbreakable(WarriorViewModel caster, float healPercent, float immortalityDuration, float stunRadius, float stunDuration)
    {
        creaturesSerivce.HealCreature(caster, caster, healPercent * caster.Stats.MaxHealth.Value);

        var immortality = new SEImmortality();
        var tmpEffect = new TemporaryStatusEffect(caster, immortality, immortalityDuration);
        caster.AddStatusEffect(tmpEffect);

        // —тан всех врагов в радиусе
        var hits = utils.DamageCircle(caster, new(), caster.Position.Value, stunRadius);

        foreach (var hit in hits)
        {
            tmpEffect = new TemporaryStatusEffect(hit.ViewModel, new SEStun(), stunDuration);
            hit.ViewModel.AddStatusEffect(tmpEffect);
        }
    }

    // ¬ынослива€ сила: увеличение урона, сопротивлени€ и защиты на определенное врем€
    public static void EnduringPower(WarriorViewModel caster, float damagePercent, float resistance, float defense, float duration)
    {
        var dmgUp = new SEPhysicalDamageChange(damagePercent, true);
        var tmpEffect = new TemporaryStatusEffect(caster, dmgUp, duration);
        caster.AddStatusEffect(tmpEffect);

        var defUp = new SEDefenseChange(defense);
        tmpEffect = new TemporaryStatusEffect(caster, defUp, duration);
        caster.AddStatusEffect(tmpEffect);

        // TODO +resistance
    }

    // ћетка палача: нанесение урона всем помеченным цел€м
    public static void ExecutionersMark(WarriorViewModel caster, float totalDamage, float duration)
    {
        var war = (WarriorViewModel)caster;

        foreach (var target in war.MarkedTargets)
        {
            target.DynamicStats.MarkCount = 0;
            target.AddStatusEffect(new SEDot(creaturesSerivce, caster, totalDamage, duration));
        }
    }

    // –азлом земли: оглушение и увеличение получаемого урона дл€ всех врагов в области
    public static void EarthRift(WarriorViewModel caster, Vector2 mousePosition, Vector2 size, float debuffDuration, float damageAmplify)
    {
        Vector2 direction = (mousePosition - caster.Position.Value).normalized;

        var points = MathUtils.GetRectPoints(size, caster.Position.Value, direction);
        var hits = Physics2DUtils.GetColliderHits<CreatureBinder>(points);

        utils.CreateRectParticle(size, points, direction);

        foreach (var hit in hits)
        {
            if (hit.ViewModel.CreatureId == caster.CreatureId) continue;

            var stunEffect = new TemporaryStatusEffect(hit.ViewModel, new SEStun(), debuffDuration);
            var amplifyEffect = new TemporaryStatusEffect(hit.ViewModel, new SEDefenseChange(damageAmplify, true), debuffDuration);
            hit.ViewModel.AddStatusEffect(stunEffect);
            hit.ViewModel.AddStatusEffect(amplifyEffect);
        }
    }

    // ќтложенное возмездие: запуск корутины дл€ выполнени€ отложенного возмезди€
    public static void DelayedReckoning(WarriorViewModel caster, float damageResistancePercent, float damageRadius)
    {
        GameEntryPoint.Coroutines.StartCoroutine(utils.DelayedReckoningCoroutine(caster, damageResistancePercent, damageRadius));
    }

    // Ѕесконечна€ €рость: метод пока не реализован
    public static void InifinteRage(WarriorViewModel caster, float staminaAttackSpeedPercent, float duration)
    {

    }
}
