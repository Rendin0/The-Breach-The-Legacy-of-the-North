using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PopupWorldMapViewModel : WindowViewModel
{
    public override string Id => "PopupWorldMap";

    public ReactiveProperty<float> Scale { get; } = new(1f);

    public PopupWorldMapViewModel()
    {
        InputRequests.EscapeRequest = new();
        InputRequests.MRequest = new();
        InputRequests.WheelRequest = new();

        InputRequests.MRequest.Subscribe(c => Close(c));
        InputRequests.EscapeRequest.Subscribe(c => Close(c));
        InputRequests.WheelRequest.Subscribe(c => ChangeScale(c));
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
            RequestClose();
    }
}