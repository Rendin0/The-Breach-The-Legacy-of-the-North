public class CmdHealCreature : ICommand
{
    public CreatureViewModel Creature { get; }
    public float Heal { get; }
    public CreatureViewModel HealDealer { get; }
    public CmdHealCreature(CreatureViewModel creature, CreatureViewModel healDealer, float heal)
    {
        Creature = creature;
        HealDealer = healDealer;
        Heal = heal;
    }
}