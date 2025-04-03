
using R3;
using System.Collections;
using UnityEngine;

public class Ability<T> : IAbility where T : CreatureViewModel
{
    public readonly string Name;

    private readonly AbilityConfig<T>.EventAbility _use;
    private readonly AbilityConfig<T>.EventAbilityRequirement _requirement;
    private readonly float _cooldownTime;

    public BoolWrapper CanUse = new();
    private bool _onCooldown = false;

    private ReactiveProperty<float> _currentCooldown = new();
    public ReactiveProperty<float> CurrentCooldown => _currentCooldown;

    private readonly Subject<IElementInfoViewModel> _onMouseEnter = new();
    private readonly Subject<IElementInfoViewModel> _onMouseExit = new();
    public Subject<IElementInfoViewModel> OnMouseEnter => _onMouseEnter;
    public Subject<IElementInfoViewModel> OnMouseExit => _onMouseExit;

    private readonly string _description;

    public string ElementName => Name;
    public string Description => _description;
    public Sprite Icon => Resources.Load<Sprite>($"UI/Abilities/{typeof(T).Name.Replace("ViewModel", "")}/{Name}");

    public Ability(AbilityConfig<T> config)
    {
        Name = config.Name;

        _description = config.Description;
        _requirement = config.Requirement;
        _use = config.Use;
        _cooldownTime = config.CooldownTime;
    }

    // Получилось либо не получилось активировать
    public bool Use<T1>(T1 viewModel, Vector2 position) where T1 : CreatureViewModel
    {
        _requirement?.Invoke(viewModel as T, CanUse);

        if (!_onCooldown && CanUse.Value)
        {
            _use.Invoke(viewModel as T, position);
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