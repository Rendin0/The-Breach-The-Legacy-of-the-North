
using R3;
using UnityEngine.InputSystem;

public class InputRequests
{
    public Subject<InputAction.CallbackContext> TabRequest { get; set; }
    public Subject<InputAction.CallbackContext> EscapeRequest { get; set; }
    public Subject<InputAction.CallbackContext> URequest { get; set; }
    public Subject<InputAction.CallbackContext> MRequest { get; set; }
    public Subject<InputAction.CallbackContext> MouseRequest { get; set; }
    public Subject<InputAction.CallbackContext> AltRequest { get; set; }

    public CompositeDisposable Subscribe(InputRequests other)
    {
        CompositeDisposable subs = new();

        TabRequest.Subscribe(_ => other.TabRequest?.OnNext(_)).AddTo(subs);
        EscapeRequest.Subscribe(_ => other.EscapeRequest?.OnNext(_)).AddTo(subs);
        URequest.Subscribe(_ => other.URequest?.OnNext(_)).AddTo(subs);
        MRequest.Subscribe(_ => other.MRequest?.OnNext(_)).AddTo(subs);
        MouseRequest.Subscribe(_ => other.MouseRequest?.OnNext(_)).AddTo(subs);
        AltRequest.Subscribe(_ => other.AltRequest?.OnNext(_)).AddTo(subs);

        return subs;
    }

    public void SetRequests(InputRequests other)
    {
        if (other.TabRequest != null)
            TabRequest = other.TabRequest;
        if (other.EscapeRequest != null)
            EscapeRequest = other.EscapeRequest;
        if (other.URequest != null)
            URequest = other.URequest;
        if (other.MRequest != null)
            MRequest = other.MRequest;
        if (other.MouseRequest != null)
            MouseRequest = other.MouseRequest;
        if (other.AltRequest != null)
            AltRequest = other.AltRequest;
    }
}