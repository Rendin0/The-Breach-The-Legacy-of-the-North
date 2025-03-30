
using UnityEngine;

public class AgentViewModel : CreatureViewModel
{
    private readonly Ability<AgentViewModel> _attack;
    public CreatureViewModel CurrentTarget { get; set; }
    public AgentTypes AgentType => creatureEntity.AgentType;



    public AgentViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig) 
        : base(creatureEntity, abilitiesConfig)
    {
        //_attack = new(abilitiesConfig.WarriorAbilitesConfig.Attack);
    }

    public bool Attack(Vector2 position)
    {
        bool result = _attack.Use(this, position);

        if (result)
            _attack.SetCooldown(DynamicStats.AttackSpeed);

        return result;
    }
}