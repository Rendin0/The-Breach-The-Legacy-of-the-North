using System;
using R3;

public abstract class WindowViewModel : IDisposable
{
    public Observable<WindowViewModel> CloseRequested => _closeRequested;
    private readonly Subject<WindowViewModel> _closeRequested = new ();

    public abstract string Id { get; }

    public void RequestClose()
    {
        _closeRequested.OnNext(this);
    }

    public virtual void Dispose()
    {
    }
}
