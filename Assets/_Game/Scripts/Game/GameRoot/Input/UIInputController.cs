using UnityEngine.InputSystem;
using static GameInput;

public class UIInputController : IUIActions
{
    private readonly InputRequests _inputRequests;
    private readonly GameInput _inputActions;

    public UIInputController(GameInput inputActions, InputRequests mainMenuInputRequests)
    {
        _inputRequests = mainMenuInputRequests;
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
        if (context.performed)
            _inputRequests.EscapeRequest.OnNext(context);
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
