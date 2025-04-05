
public class HunterAgentViewModel : AgentViewModel
{
    public HunterAgentViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig) : base(creatureEntity)
    {
        abilitiesConfig.HunterAbilitiesConfig.Attacks.ForEach(attack => Attacks.Add(new Ability<HunterAgentViewModel>(attack)));
        abilitiesConfig.HunterAbilitiesConfig.Heals.ForEach(heal => Heals.Add(new Ability<HunterAgentViewModel>(heal)));

    }
}