
public class CmdAddThreatHandler : ICommandHandler<CmdAddThreat>
{
    public bool Handle(CmdAddThreat command)
    {
        return AddThreat(command.Creature, command.ThreatDealer, command.Threat);
    }

    private bool AddThreat(CreatureViewModel creature, CreatureViewModel threatDealer, float threat)
    {
        if (creature is AgentViewModel agent)
        {
            agent.ThreatMap[threatDealer] = threat;
            return true;
        }

        return false;
    }
}