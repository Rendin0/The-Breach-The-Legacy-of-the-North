using R3;
using System;

public class ScreenGameplayViewModel : WindowViewModel
{
    private readonly GameplayUIManager _uiManager;

    public override string Id => "ScreenGameplay";

    public ScreenGameplayViewModel(GameplayUIManager uiManager)
    {
        this._uiManager = uiManager;
        EscapeRequest.Subscribe(_ => RequestPause());
        TabRequest.Subscribe(_ => RequestInventory(0));
        
    }

    public void RequestPause()
    {
        _uiManager.OpenScreenGameplayPause();
    }

    public void RequestInventory(int ownerId)
    {
        _uiManager.OpenPopupInventory(ownerId, this);
    }
}