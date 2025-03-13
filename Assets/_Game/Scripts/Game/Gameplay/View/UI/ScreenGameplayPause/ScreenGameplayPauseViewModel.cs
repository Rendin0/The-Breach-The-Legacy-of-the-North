using R3;
using UnityEngine.InputSystem;

public class ScreenGameplayPauseViewModel : WindowViewModel
{
    private readonly GameplayUIManager _uiManager;
    private readonly Subject<Unit> _exitSceneRequest;

    public override string Id => "ScreenGameplayPause";

    public ScreenGameplayPauseViewModel(GameplayUIManager uiManager, Subject<Unit> exitSceneRequest)
    {
        _exitSceneRequest = exitSceneRequest;
        _uiManager = uiManager;

        InputRequests.EscapeRequest = new();

        InputRequests.EscapeRequest.Subscribe(_ => RequestResume(_));
    }

    public void RequestOpenPopupSettings()
    {
        _uiManager.OpenPopupSettings();
    }

    public void RequestReturnToMenu()
    {
        _exitSceneRequest.OnNext(Unit.Default);
    }

    public void RequestResume(InputAction.CallbackContext context)
    {
        if (context.performed)
            RequestResume();
    }
    public void RequestResume()
    {
        _uiManager.OpenScreenGameplay();
    }
}