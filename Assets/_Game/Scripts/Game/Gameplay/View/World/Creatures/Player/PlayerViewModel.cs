
using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerViewModel : WarriorViewModel, IControllable
{
    public ReactiveProperty<Vector2> MoveDirection { get; } = new();
    public readonly List<Ability> Abilities = new();
    public ReactiveProperty<(float scale, Vector2 position)> MapState => creatureEntity.MapState;
    private readonly Ability _attack;


    public PlayerViewModel(CreatureEntityProxy creatureEntity, AbilitiesConfig abilitiesConfig)
        : base(creatureEntity, abilitiesConfig)
    {
        foreach (var abilityCfg in abilitiesConfig.Abilities)
        {
            Abilities.Add(new(abilityCfg));
        }
        _attack = new(abilitiesConfig.Attack);
    }

    public bool Attack(Vector2 position)
    {
        GameEntryPoint.Coroutines.StartCoroutine(AttackCoroutine(position));
        return true;
    }

    private IEnumerator AttackCoroutine(Vector2 position)
    {
        // Скип одного кадра, чтобы IsPointerOverGameObject сработал правильно
        yield return null;
        if (!EventSystem.current.IsPointerOverGameObject())
            if (_attack.Use(this, position))
                _attack.SetCooldown(DynamicStats.AttackSpeed);
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