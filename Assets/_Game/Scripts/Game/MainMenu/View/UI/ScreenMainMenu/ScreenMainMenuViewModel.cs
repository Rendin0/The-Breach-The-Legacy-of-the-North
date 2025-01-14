
using R3;

public class ScreenMainMenuViewModel : WindowViewModel
{
    private readonly MainMenuUIManager _uiManager;
    private readonly Subject<Unit> _exitSceneRequest;

    public override string Id => "ScreenMainMenu";

    public ScreenMainMenuViewModel(MainMenuUIManager uiManager, Subject<Unit> exitSceneRequest)
    {
        this._uiManager = uiManager;
        this._exitSceneRequest = exitSceneRequest;
    }

    public void RequestOpenPopupSettings()
    {
        _uiManager.OpenPopupSettings();
    }

    public void RequestPlay()
    {
        _exitSceneRequest.OnNext(Unit.Default);
    }

    public void RequestExitGame()
    {
        _uiManager.ExitGame();
    }
}