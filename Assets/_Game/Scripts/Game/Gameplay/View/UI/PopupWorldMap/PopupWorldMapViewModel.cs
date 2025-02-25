using R3;
using UnityEngine.InputSystem;

public class PopupWorldMapViewModel : WindowViewModel
{
    public override string Id => "PopupWorldMap";

    public PopupWorldMapViewModel()
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
