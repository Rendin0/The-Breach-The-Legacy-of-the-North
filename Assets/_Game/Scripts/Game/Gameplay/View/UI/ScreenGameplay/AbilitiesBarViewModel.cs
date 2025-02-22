
using R3;
using System;
using System.Collections.Generic;

public class AbilitiesBarViewModel : IElementInfoViewModel, IDisposable
{
    public readonly List<Ability> Abilities = new();

    public AbilitiesBarViewModel(PlayerViewModel player)
    {
        foreach (var ability in player.Abilities)
        {
            Abilities.Add(ability);

            _subs.Add(ability.OnMouseEnter.Subscribe(e => OnMouseEnter.OnNext(e)));
            _subs.Add(ability.OnMouseExit.Subscribe(e => OnMouseExit.OnNext(e)));
        }
    }

    private readonly CompositeDisposable _subs = new();
    private readonly Subject<IElementInfoViewModel> _onMouseExit = new();
    private readonly Subject<IElementInfoViewModel> _onMouseEnter = new();
    public Subject<IElementInfoViewModel> OnMouseEnter => _onMouseEnter;
    public Subject<IElementInfoViewModel> OnMouseExit => _onMouseExit;

    public string Text => "";

    public void Dispose()
    {
        _subs.Dispose();
    }
}
