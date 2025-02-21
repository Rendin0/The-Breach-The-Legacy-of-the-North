using R3;

public class ScreenGameplayViewModel : WindowViewModel
{
    public override string Id => "ScreenGameplay";
    private readonly GameplayUIManager _uiManager;

    public readonly AbilitiesBarViewModel AbilitiesBarViewModel;
    public readonly PlayerStatsViewModel playerStatsViewModel;

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
}