
public class CmdDamageCreature : ICommand
{
    public readonly CreatureViewModel Creature;
    public readonly DamageData Damage;

    public CmdDamageCreature(CreatureViewModel creature, DamageData damage)
    {
        Creature = creature;
        Damage = damage;
    }
}