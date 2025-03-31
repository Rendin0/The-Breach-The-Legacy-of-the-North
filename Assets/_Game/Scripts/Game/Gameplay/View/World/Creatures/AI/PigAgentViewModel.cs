
public class PigAgentViewModel : AgentViewModel
{
    public PigAgentViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig) 
        : base(creatureEntity)
    {
        foreach (var abilityCfg in abilitiesConfig.PigAbilitiesConfig.Abilities)
        {
            Abilities.Add(new Ability<PigAgentViewModel>(abilityCfg));
        }
    }
}