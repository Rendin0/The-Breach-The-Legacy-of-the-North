
public class CmdDamageCreature : ICommand
{
    public readonly CreatureViewModel Creature;
    public readonly CreatureViewModel DamageDealer;
    public readonly DamageData Damage;

    public CmdDamageCreature(CreatureViewModel creature, CreatureViewModel damageDealer, DamageData damage)
    {
        Creature = creature;
        DamageDealer = damageDealer;
        Damage = damage;
    }
}