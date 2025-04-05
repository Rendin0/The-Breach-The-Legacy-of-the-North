
public class CmdAddThreatHandler : ICommandHandler<CmdAddThreat>
{
    public bool Handle(CmdAddThreat command)
    {
        return AddThreat(command.Creature, command.ThreatDealer, command.Threat);
    }

    private bool AddThreat(CreatureViewModel creature, CreatureViewModel threatDealer, float threat)
    {
        if (creature is not AgentViewModel agent)
            return false;

        if (agent.ThreatMap.ContainsKey(threatDealer))
            agent.ThreatMap[threatDealer] += threat;
        else
            agent.ThreatMap[threatDealer] = threat;

        return true;
    }
}