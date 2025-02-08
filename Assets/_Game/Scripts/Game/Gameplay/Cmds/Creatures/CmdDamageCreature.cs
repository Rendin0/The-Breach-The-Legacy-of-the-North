
public class CmdDamageCreature : ICommand
{
    public readonly CreatureViewModel Creature;
    public readonly float Damage;

    public CmdDamageCreature(CreatureViewModel creature, float damage)
    {
        Creature = creature;
        Damage = damage;
    }
}