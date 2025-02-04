using R3;

public class ScreenGameplayViewModel : WindowViewModel
{
    private readonly GameplayUIManager _uiManager;

    public override string Id => "ScreenGameplay";

    public ScreenGameplayViewModel(GameplayUIManager uiManager)
    {
        this._uiManager = uiManager;

        InputRequests.EscapeRequest = new();
        InputRequests.TabRequest = new();
        InputRequests.URequest = new();

        InputRequests.EscapeRequest.Subscribe(_ => RequestPause());
        InputRequests.TabRequest.Subscribe(_ => RequestInventory(0));
        InputRequests.URequest.Subscribe(_ => RequestDevPanel());
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