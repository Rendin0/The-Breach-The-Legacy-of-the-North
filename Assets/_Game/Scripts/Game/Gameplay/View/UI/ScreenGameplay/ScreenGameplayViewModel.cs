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

    public Subject<IElementInfoViewModel> CreateElementInfo = new();
    public Subject<IElementInfoViewModel> DeleteElementInfo = new();

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
        AbilitiesBarViewModel.OnMouseEnter.Subscribe(e => CreateElementInfo.OnNext(e)).AddTo(_subs);
        AbilitiesBarViewModel.OnMouseExit.Subscribe(e => DeleteElementInfo.OnNext(e)).AddTo(_subs);
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