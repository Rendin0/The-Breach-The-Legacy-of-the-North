
public class CmdDamageCreature : ICommand
{
    public readonly int CreatureId;
    public readonly float Damage;

    public CmdDamageCreature(int creatureId, float damage)
    {
        CreatureId = creatureId;
        Damage = damage;
    }
}