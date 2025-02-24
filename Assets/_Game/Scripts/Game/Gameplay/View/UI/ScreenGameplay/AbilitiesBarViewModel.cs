
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilitiesBarViewModel : IElementInfoViewModel, IDisposable
{
    public readonly List<Ability> Abilities = new();
    public readonly List<string> AbilityBindings = new();


    public AbilitiesBarViewModel(PlayerViewModel player, InputAction bindings)
    {
        foreach (var ability in player.Abilities)
        {
            Abilities.Add(ability);

            _subs.Add(ability.OnMouseEnter.Subscribe(e => OnMouseEnter.OnNext(e)));
            _subs.Add(ability.OnMouseExit.Subscribe(e => OnMouseExit.OnNext(e)));
        }

        foreach (var binding in bindings.controls)
        {
            AbilityBindings.Add(binding.displayName);
        }
    }

    private readonly CompositeDisposable _subs = new();
    private readonly Subject<IElementInfoViewModel> _onMouseExit = new();
    private readonly Subject<IElementInfoViewModel> _onMouseEnter = new();
    public Subject<IElementInfoViewModel> OnMouseEnter => _onMouseEnter;
    public Subject<IElementInfoViewModel> OnMouseExit => _onMouseExit;

    public readonly Subject<InputAction.CallbackContext> AddExtraBar = new();

    public Sprite Icon => throw new NotImplementedException();
    public string ElementName => throw new NotImplementedException();
    public string Description => throw new NotImplementedException();

    public void Dispose()
    {
        _subs.Dispose();
    }
}
