
using R3;
using UnityEngine;

public interface IAbility : IElementInfoViewModel
{
    public bool Use<T>(T viewModel, Vector2 position) where T : CreatureViewModel;
    public void SetCooldown(float cooldown, bool priority = false);
    public ReactiveProperty<float> CurrentCooldown { get; }

}