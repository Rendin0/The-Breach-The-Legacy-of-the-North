
using R3;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewModel : CreatureViewModel, IControllable
{
    public ReactiveProperty<Vector2> MoveDirection { get; } = new();
    private readonly List<Ability> _abilities = new();

    public PlayerViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig)
        : base(creatureEntity)
    {
        foreach (var abilityCfg in abilitiesConfig.Abilities)
        {
            _abilities.Add(new(abilityCfg));
        }
    }

    public void UseAbility(int index, Vector2 position)
    {
        if (index >= _abilities.Count)
            return;

        _abilities[index].Use.Invoke(this, position);
    }

    public void Move(Vector2 direction)
    {
        MoveDirection.OnNext(direction);
    }
}