using R3;

public class ScreenGameplayPauseViewModel : WindowViewModel
{
    private readonly GameplayUIManager _uiManager;
    private readonly Subject<Unit> _exitSceneRequest;

    public override string Id => "ScreenGameplayPause";

    public ScreenGameplayPauseViewModel(GameplayUIManager uiManager, Subject<Unit> exitSceneRequest)
    {
        _exitSceneRequest = exitSceneRequest;
        _uiManager = uiManager;
    }

    public void RequestOpenPopupSettings()
    {
        _uiManager.OpenPopupSettings();
    }

    public void RequestReturnToMenu()
    {
        _exitSceneRequest.OnNext(Unit.Default);
    }

    public void RequestResume()
    {

    }
}