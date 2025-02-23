
using R3;
using UnityEngine.InputSystem;
using static GameInput;

public class UIInputController : IUIActions
{
    private readonly Subject<InputAction.CallbackContext> _escapeRequest;
    private readonly GameInput _inputActions;

    public UIInputController(GameInput inputActions, Subject<InputAction.CallbackContext> escapeRequest)
    {
        _escapeRequest = escapeRequest;
        _inputActions = inputActions;
        _inputActions.UI.SetCallbacks(this);
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
    }

    public void OnClick(InputAction.CallbackContext context)
    {
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        _escapeRequest.OnNext(context);
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
    }
}
