using R3;
using UnityEngine;
using UnityEngine.EventSystems;
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
            _inputRequests.EscapeRequest.OnNext(Unit.Default);
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
            _inputRequests.TabRequest.OnNext(Unit.Default);
    }

    public void OnU(InputAction.CallbackContext context)
    {
        if (context.performed)
            _inputRequests.URequest.OnNext(Unit.Default);
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        if (context.performed /*&& !EventSystem.current.IsPointerOverGameObject()*/)
        {
            _controllable.Attack(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _inputRequests.MouseRequest.OnNext(Unit.Default);
        }
    }

    public void OnAbilities(InputAction.CallbackContext context)
    {
        if (context.performed)
            _controllable.UseAbility(context.action.GetBindingIndexForControl(context.control), Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
