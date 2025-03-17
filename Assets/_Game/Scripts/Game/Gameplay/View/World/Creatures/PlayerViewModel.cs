
using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerViewModel : WarriorViewModel, IControllable
{
    public ReactiveProperty<Vector2> MoveDirection { get; } = new();
    public readonly List<Ability> Abilities = new();

    public PlayerViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig)
        : base(creatureEntity, abilitiesConfig)
    {
        foreach (var abilityCfg in abilitiesConfig.Abilities)
        {
            Abilities.Add(new(abilityCfg));
        }
        attack = new(abilitiesConfig.Attack);
    }

    public override bool Attack(Vector2 position)
    {
        GameEntryPoint.Coroutines.StartCoroutine(AttackCoroutine(position));
        return true;
    }

    private IEnumerator AttackCoroutine(Vector2 position)
    {
        // ���� ������ �����, ����� IsPointerOverGameObject �������� ���������
        yield return null;
        if (!EventSystem.current.IsPointerOverGameObject())
            if (attack.Use(this, position))
                attack.SetCooldown(DynamicStats.AttackSpeed);
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