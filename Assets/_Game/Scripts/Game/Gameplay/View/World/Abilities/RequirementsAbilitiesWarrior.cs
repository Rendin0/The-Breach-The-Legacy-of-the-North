
public static class RequirementsAbilitiesWarrior
{
    public static void UnbreakableRequirements(CreatureViewModel caster, BoolWrapper result)
    {
        // ≈сли кол-во урона за последние 5 секунд больше половины макисимума хп, то можно использовать
        result.Value = caster.DynamicStats.HealthChanges >= (caster.Stats.MaxHealth.Value / 2f);
    }

    public static void ExecutionersMarkRequirements(CreatureViewModel caster, BoolWrapper result)
    {
        var war = (WarriorViewModel)caster;
        result.Value = war.MarkedTargets.Count > 0;
    }

}