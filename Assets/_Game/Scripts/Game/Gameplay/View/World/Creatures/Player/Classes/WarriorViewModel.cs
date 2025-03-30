using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.EventSystems;

public class WarriorViewModel : PlayerViewModel
{
    public List<CreatureViewModel> MarkedTargets { get; } = new();
    public bool InfiniteRage = false;


    

    public WarriorViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig) 
        : base(creatureEntity, abilitiesConfig)
    {
        foreach (var abilityCfg in abilitiesConfig.WarriorAbilitesConfig.Abilities)
        {
            Abilities.Add(new Ability<WarriorViewModel>(abilityCfg));
        }
        AttackAbility = new Ability<WarriorViewModel>(abilitiesConfig.WarriorAbilitesConfig.Attack);
    }

}