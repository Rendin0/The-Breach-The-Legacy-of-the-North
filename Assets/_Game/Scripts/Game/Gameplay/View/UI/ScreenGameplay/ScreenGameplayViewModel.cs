using R3;
using System.Collections.Generic;
using System.Linq;

public class ScreenGameplayViewModel : WindowViewModel
{
    public override string Id => "ScreenGameplay";

    private readonly GameplayUIManager _uiManager;

    public readonly AbilitiesBarViewModel AbilitiesBarViewModel;
    public readonly PlayerStatsViewModel playerStatsViewModel;

    private readonly CompositeDisposable _subs = new();
    private readonly List<(IElementInfoViewModel, PopupElementInfoViewModel)> _elementInfoWindows = new();

    public ScreenGameplayViewModel(GameplayUIManager uiManager, PlayerViewModel player)
    {
        this._uiManager = uiManager;

        playerStatsViewModel = new(player);

        InputRequests.EscapeRequest = new();
        InputRequests.TabRequest = new();
        InputRequests.URequest = new();

        InputRequests.EscapeRequest.Subscribe(_ => RequestPause());
        InputRequests.TabRequest.Subscribe(_ => RequestInventory(0));
        InputRequests.URequest.Subscribe(_ => RequestDevPanel());

        AbilitiesBarViewModel = new(player);
        AbilitiesBarViewModel.OnMouseEnter.Subscribe(e => OpenElementInfo(e)).AddTo(_subs);
        AbilitiesBarViewModel.OnMouseExit.Subscribe(e => CloseElementInfo(e)).AddTo(_subs);
    }

    private void OpenElementInfo(IElementInfoViewModel element)
    {
        _elementInfoWindows.Add((element, _uiManager.OpenPopupElementInfo(element)));
    }

    private void CloseElementInfo(IElementInfoViewModel element)
    {
        var window = _elementInfoWindows.FirstOrDefault(e => e.Item1 == element);

        if (window.Item2 != null)
        {
            _uiManager.ClosePopupElementInfo(window.Item2);
        }

        _elementInfoWindows.Remove(window);
    }

    private void RequestPause()
    {
        _uiManager.OpenScreenGameplayPause();
    }

    private void RequestInventory(int ownerId)
    {
        _uiManager.OpenPopupInventory(ownerId);
    }

    private void RequestDevPanel()
    {
        _uiManager.OpenPopupDevPanel();
    }

    public override void Dispose()
    {
        base.Dispose();

        _subs.Dispose();
        AbilitiesBarViewModel.Dispose();
    }
}