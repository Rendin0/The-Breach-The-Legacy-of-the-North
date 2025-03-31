using System.Collections.Generic;

public class WarriorViewModel : PlayerViewModel
{
    public List<CreatureViewModel> MarkedTargets { get; } = new();
    public bool InfiniteRage = false;




    public WarriorViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig)
        : base(creatureEntity)
    {
        foreach (var abilityCfg in abilitiesConfig.WarriorAbilitesConfig.Abilities)
        {
            Abilities.Add(new Ability<WarriorViewModel>(abilityCfg));
        }
        AttackAbility = new Ability<WarriorViewModel>(abilitiesConfig.WarriorAbilitesConfig.Attack);
    }

}