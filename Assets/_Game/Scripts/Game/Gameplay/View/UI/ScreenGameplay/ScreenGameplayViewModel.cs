public class ScreenGameplayViewModel : WindowViewModel
{
    private readonly GameplayUIManager _uiManager;

    public override string Id => "ScreenGameplay";

    public ScreenGameplayViewModel(GameplayUIManager uiManager)
    {
        this._uiManager = uiManager;
    }

    public void RequestPause()
    {
        _uiManager.OpenScreenGameplayPause();
    }

    public void RequestInventory(int ownerId)
    {
        _uiManager.OpenPopupInventory(ownerId);
    }
}