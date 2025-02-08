
using System.Collections.Generic;

public class AbilitiesBarViewModel
{
    public readonly List<Ability> Abilities = new();

    public AbilitiesBarViewModel(PlayerViewModel player)
    {
        foreach (var ability in player.Abilities)
        {
            Abilities.Add(ability);
        }
    }

}
