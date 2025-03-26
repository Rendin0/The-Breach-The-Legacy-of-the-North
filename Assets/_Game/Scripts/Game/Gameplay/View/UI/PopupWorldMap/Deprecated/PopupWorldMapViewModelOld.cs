using R3;
using System;
using UnityEngine.InputSystem;

[Obsolete("Old map script")]
public class PopupWorldMapViewModelOld : WindowViewModel
{
    public override string Id => "PopupWorldMap";

    public PopupWorldMapViewModelOld()
    {
        InputRequests.EscapeRequest = new();
        InputRequests.MRequest = new();

        InputRequests.MRequest.Subscribe(c => Close(c));
    }

    private void Close(InputAction.CallbackContext context)
    {
        if (context.performed)
            RequestClose();
    }
}
