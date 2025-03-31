
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentViewModel : CreatureViewModel
{
    public readonly List<IAbility> Abilities = new();
    public CreatureViewModel CurrentTarget { get; set; }
    public AgentTypes AgentType => creatureEntity.AgentType;



    public AgentViewModel(CreatureEntityProxy creatureEntity) 
        : base(creatureEntity)
    {
        //_attack = new(abilitiesConfig.WarriorAbilitesConfig.Attack);
    }

    public void UseAbility(int index, Vector2 position)
    {
        if (index >= Abilities.Count)
            return;

        if (Abilities[index].Use(this, position))
        {
            // Перезарядка всем остальным на одну секунду, чтобы не было спама
            foreach (var ability in Abilities)
                ability.SetCooldown(1f);
        }
    }
}