
using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerViewModel : CreatureViewModel, IControllable
{
    public ReactiveProperty<Vector2> MoveDirection { get; } = new();
    public readonly List<Ability> Abilities = new();
    protected readonly Ability attack;

    public PlayerViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig)
        : base(creatureEntity)
    {
        foreach (var abilityCfg in abilitiesConfig.Abilities)
        {
            Abilities.Add(new(abilityCfg));
        }
        attack = new(abilitiesConfig.Attack);
    }

    public void Attack(Vector2 position)
    {
        GameEntryPoint.Coroutines.StartCoroutine(AttackCoroutine(position));
    }

    private IEnumerator AttackCoroutine(Vector2 position)
    {
        // Скип одного кадра, чтобы IsPointerOverGameObject сработал правильно
        yield return null;
        if (!EventSystem.current.IsPointerOverGameObject())
            if (attack.Use(this, position))
                attack.SetCooldown(AttackSpeed);
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

    public void Move(Vector2 direction)
    {
        MoveDirection.OnNext(direction);
    }
}