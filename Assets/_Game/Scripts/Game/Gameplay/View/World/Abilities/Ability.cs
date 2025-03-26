
using R3;
using System.Collections;
using UnityEngine;

public class Ability : IElementInfoViewModel
{
    public readonly string Name;

    private readonly EventAbility _use;
    private readonly EventAbilityRequirement _requirement;
    private readonly float _cooldownTime;

    public BoolWrapper CanUse = new();
    private bool _onCooldown = false;
    public ReactiveProperty<float> CurrentCooldown { get; } = new(0f);

    private readonly Subject<IElementInfoViewModel> _onMouseEnter = new();
    private readonly Subject<IElementInfoViewModel> _onMouseExit = new();
    public Subject<IElementInfoViewModel> OnMouseEnter => _onMouseEnter;
    public Subject<IElementInfoViewModel> OnMouseExit => _onMouseExit;

    private readonly string _description;

    public string ElementName => Name;
    public string Description => _description;
    public Sprite Icon => Resources.Load<Sprite>($"UI/Abilities/{Name}");

    public Ability(AbilityConfig config)
    {
        Name = config.Name;

        _description = config.Description;
        _requirement = config.Requirement;
        _use = config.Use;
        _cooldownTime = config.CooldownTime;
    }

    // Получилось либо не получилось активировать
    public bool Use(CreatureViewModel playerViewModel, Vector2 position)
    {
        _requirement?.Invoke(playerViewModel, CanUse);

        if (!_onCooldown && CanUse.Value)
        {
            _use.Invoke(playerViewModel, position);
            GameEntryPoint.Coroutines.StartCoroutine(CooldownTimer(_cooldownTime));
            return true;
        }
        return false;
    }

    public void SetCooldown(float cooldown, bool priority = false)
    {
        // Заменяем перезарядку только если новая больше
        // Либо если есть приоритет
        if (cooldown > CurrentCooldown.Value || priority)
        {
            if (!_onCooldown)
                GameEntryPoint.Coroutines.StartCoroutine(CooldownTimer(cooldown));
            else
                CurrentCooldown.OnNext(cooldown);
        }
    }

    // Потенциально плохая оптимизация
    private IEnumerator CooldownTimer(float duration)
    {
        _onCooldown = true;

        // Таймер перезарядки, в СurrentCooldown хранится текущее оставшееся время
        CurrentCooldown.OnNext(duration);
        while (CurrentCooldown.Value > 0)
        {
            CurrentCooldown.OnNext(CurrentCooldown.Value - Time.deltaTime);
            yield return null;
        }

        _onCooldown = false;
    }
}