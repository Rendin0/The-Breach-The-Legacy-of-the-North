
public class CmdAddThreat : ICommand
{
    public CreatureViewModel Creature { get; }
    public CreatureViewModel ThreatDealer { get; }
    public float Threat { get; }

    public CmdAddThreat(CreatureViewModel creature, CreatureViewModel threatDealer, float threat)
    {
        Creature = creature;
        ThreatDealer = threatDealer;
        Threat = threat;
    }
}
