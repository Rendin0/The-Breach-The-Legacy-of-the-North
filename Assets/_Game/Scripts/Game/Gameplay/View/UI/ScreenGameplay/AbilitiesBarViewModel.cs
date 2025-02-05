
using System.Collections.Generic;

public class AbilitiesBarViewModel
{
    public readonly List<Ability> Abilities = new();

    public AbilitiesBarViewModel(AbilitiesConfig abilitiesConfig)
    {
        foreach (var abilityCfg in abilitiesConfig.Abilities)
        {
            Abilities.Add(new(abilityCfg));
        }
    }

}
