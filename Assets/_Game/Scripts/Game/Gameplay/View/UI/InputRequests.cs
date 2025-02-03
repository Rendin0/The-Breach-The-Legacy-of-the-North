
using R3;

public class InputRequests
{
    public Subject<Unit> TabRequest { get; set; }
    public Subject<Unit> EscapeRequest { get; set; }
    public Subject<Unit> URequest { get; set; }
    public Subject<Unit> MouseRequest { get; set; }

    public CompositeDisposable Subscribe(InputRequests other)
    {
        CompositeDisposable subs = new();

        TabRequest.Subscribe(_ => other.TabRequest?.OnNext(Unit.Default)).AddTo(subs);
        EscapeRequest.Subscribe(_ => other.EscapeRequest?.OnNext(Unit.Default)).AddTo(subs);
        URequest.Subscribe(_ => other.URequest?.OnNext(Unit.Default)).AddTo(subs);
        MouseRequest.Subscribe(_ => other.MouseRequest?.OnNext(Unit.Default)).AddTo(subs);

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
        if (other.MouseRequest != null)
            MouseRequest = other.MouseRequest;
    }
}