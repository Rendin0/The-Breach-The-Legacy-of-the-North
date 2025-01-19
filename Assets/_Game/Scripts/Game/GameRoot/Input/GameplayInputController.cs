using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameInput;

public class GameplayInputController : IPlayerActions
{
    private IControllable _controllable;
    private GameInput _inputActions;
    private readonly Subject<Unit> _escapeRequest;
    private readonly Subject<Unit> _tabRequest;

    public GameplayInputController(GameInput inputActions, Subject<Unit> escapeRequest, Subject<Unit> tabRequest)
    {
        _inputActions = inputActions;
        this._escapeRequest = escapeRequest;
        this._tabRequest = tabRequest;
        _inputActions.Player.SetCallbacks(this);
    }

    public void Bind(IControllable controllable)
    {
        _controllable = controllable;
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
            _escapeRequest.OnNext(Unit.Default);
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
            _tabRequest.OnNext(Unit.Default);
    }
}
