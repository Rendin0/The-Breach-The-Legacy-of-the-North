using R3;
using UnityEngine.InputSystem;

public class ScreenGameplayViewModel : WindowViewModel
{
    public override string Id => "ScreenGameplay";

    private readonly GameplayUIManager _uiManager;

    public readonly AbilitiesBarViewModel AbilitiesBarViewModel;
    public readonly PlayerStatsViewModel playerStatsViewModel;

    private readonly CompositeDisposable _subs = new();

    public Subject<IElementInfoViewModel> CreateElementInfo = new();
    public Subject<IElementInfoViewModel> DeleteElementInfo = new();

    public ScreenGameplayViewModel(GameplayUIManager uiManager, InputAction abilityBindings, PlayerViewModel player)
    {
        this._uiManager = uiManager;

        playerStatsViewModel = new(player);

        InputRequests.EscapeRequest = new();
        InputRequests.TabRequest = new();
        InputRequests.URequest = new();
        InputRequests.MRequest = new();
        InputRequests.AltRequest = new();

        InputRequests.EscapeRequest.Subscribe(_ => RequestPause(_));
        InputRequests.TabRequest.Subscribe(_ => RequestInventory(_, 0));
        InputRequests.URequest.Subscribe(_ => RequestDevPanel(_));
        InputRequests.MRequest.Subscribe(_ => RequestWorldMap(_, player));
        InputRequests.AltRequest.Subscribe(_ => RequestSwitchAbilityBar(_));

        AbilitiesBarViewModel = new(player, abilityBindings);
        AbilitiesBarViewModel.OnMouseEnter.Subscribe(e => CreateElementInfo.OnNext(e)).AddTo(_subs);
        AbilitiesBarViewModel.OnMouseExit.Subscribe(e => DeleteElementInfo.OnNext(e)).AddTo(_subs);
    }

    private void RequestSwitchAbilityBar(InputAction.CallbackContext context)
    {
        AbilitiesBarViewModel.AddExtraBar.OnNext(context);
    }

    private void RequestPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            _uiManager.OpenScreenGameplayPause();
    }

    private void RequestWorldMap(InputAction.CallbackContext context, PlayerViewModel player)
    {
        if (context.performed)
            _uiManager.OpenPopupWorldMap(player);
    }

    private void RequestInventory(InputAction.CallbackContext context, int ownerId)
    {
        if (context.performed)
            _uiManager.OpenPopupInventory(ownerId);
    }

    private void RequestDevPanel(InputAction.CallbackContext context)
    {
        if (context.performed)
            _uiManager.OpenPopupDevPanel();
    }

    public override void Dispose()
    {
        base.Dispose();

        _subs.Dispose();
        AbilitiesBarViewModel.Dispose();
    }
}