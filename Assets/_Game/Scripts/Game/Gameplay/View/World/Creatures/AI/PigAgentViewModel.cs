
public class PigAgentViewModel : AgentViewModel
{
    public PigAgentViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig)
        : base(creatureEntity)
    {
        abilitiesConfig.PigAbilitiesConfig.Attacks.ForEach(attack => Attacks.Add(new Ability<PigAgentViewModel>(attack)));
        abilitiesConfig.PigAbilitiesConfig.Heals.ForEach(heal => Heals.Add(new Ability<PigAgentViewModel>(heal)));
    }
}