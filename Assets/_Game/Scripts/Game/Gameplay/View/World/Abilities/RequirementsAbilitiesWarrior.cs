
public static class RequirementsAbilitiesWarrior
{
    public static void UnbreakableRequirements(CreatureViewModel caster, BoolWrapper result)
    {
        // ���� ���-�� ����� �� ��������� 5 ������ ������ �������� ���������� ��, �� ����� ������������
        result.Value = caster.HealthChanges >= (caster.Stats.MaxHealth.Value / 2f);
    }

    public static void ExecutionersMarkRequirements(CreatureViewModel caster, BoolWrapper result)
    {
        result.Value = caster.MarkedTargets.Count > 0;
    }

}