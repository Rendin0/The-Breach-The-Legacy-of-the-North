using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameInput;

public class InputController : IPlayerActions
{
    private IControllable _controllable;
    private GameInput _inputActions;

    public InputController(GameInput inputActions)
    {
        _inputActions = inputActions;
        _inputActions.Player.SetCallbacks(this);

    }

    public void Bind(IControllable controllable)
    {
        _controllable = controllable;
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
}
