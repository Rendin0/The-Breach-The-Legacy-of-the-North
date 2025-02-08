
using R3;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewModel : CreatureViewModel, IControllable
{
    public ReactiveProperty<Vector2> MoveDirection { get; } = new();
    public readonly List<Ability> Abilities = new();

    public PlayerViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig)
        : base(creatureEntity)
    {
        foreach (var abilityCfg in abilitiesConfig.Abilities)
        {
            Abilities.Add(new(abilityCfg));
        }
    }

    public void UseAbility(int index, Vector2 position)
    {
        if (index >= Abilities.Count)
            return;

        if (Abilities[index].Use(this, position))
        {
            // ����������� ���� ��������� �� ���� �������, ����� �� ���� �����
            foreach (var ability in Abilities)
                ability.SetCooldown(1f);
        }
    }

    public void Move(Vector2 direction)
    {
        MoveDirection.OnNext(direction);
    }
}