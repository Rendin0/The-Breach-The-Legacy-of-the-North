using UnityEngine;
using UnityEngine.InputSystem;
using static GameInput;

public class GameplayInputController : IPlayerActions
{
    private IControllable _controllable;
    private GameInput _inputActions;
    private readonly InputRequests _inputRequests = new();

    public GameplayInputController(GameInput inputActions, InputRequests inputRequests)
    {
        _inputActions = inputActions;
        _inputRequests.SetRequests(inputRequests);

        _inputActions.Player.SetCallbacks(this);
    }

    public void Bind(IControllable controllable)
    {
        _controllable = controllable;
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
            _inputRequests.EscapeRequest.OnNext(context);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        //
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _controllable?.Move(context.ReadValue<Vector2>());
    }

    public void OnTab(InputAction.CallbackContext context)
    {
        if (context.performed)
            _inputRequests.TabRequest.OnNext(context);
    }

    public void OnU(InputAction.CallbackContext context)
    {
        if (context.performed)
        _inputRequests.URequest.OnNext(context);
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _controllable.Attack(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        _inputRequests.MouseRequest.OnNext(context);
    }

    public void OnAbilities(InputAction.CallbackContext context)
    {
        if (context.performed)
            _controllable.UseAbility(context.action.GetBindingIndexForControl(context.control), Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public void OnAlt(InputAction.CallbackContext context)
    {
        _inputRequests.AltRequest.OnNext(context);
    }

    public void OnM(InputAction.CallbackContext context)
    {
        if (context.performed)
            _inputRequests.MRequest.OnNext(context);
    }

    public void OnWheel(InputAction.CallbackContext context)
    {
        if (context.performed)
            _inputRequests.WheelRequest.OnNext(context);
    }
}
