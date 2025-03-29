using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PopupWorldMapViewModel : WindowViewModel
{
    private readonly PlayerViewModel _player;

    public override string Id => "PopupWorldMap";

    public ReactiveProperty<float> Scale { get; }
    public ReactiveProperty<Vector2> Position { get; }

    public PopupWorldMapViewModel(PlayerViewModel player)
    {
        InputRequests.EscapeRequest = new();
        InputRequests.MRequest = new();
        InputRequests.WheelRequest = new();

        InputRequests.MRequest.Subscribe(c => Close(c));
        InputRequests.EscapeRequest.Subscribe(c => Close(c));
        InputRequests.WheelRequest.Subscribe(c => ChangeScale(c));

        Scale = new(player.MapState.Value.scale);
        Position = new(player.MapState.Value.position);
        this._player = player;
    }

    private void ChangeScale(InputAction.CallbackContext context)
    {
        float changeAmount = context.ReadValue<float>() * .05f;

        // InputSystem почему-то всегда выдаёт два значения: ноль и значение направления
        if (changeAmount != 0)
            Scale.OnNext(Mathf.Clamp(Scale.Value - changeAmount, .05f, 1f));
    }

    private void Close(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RequestClose();
            _player.MapState.OnNext((Scale.Value, Position.Value));

        }
    }
}