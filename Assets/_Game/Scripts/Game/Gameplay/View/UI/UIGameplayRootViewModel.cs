using R3;

public class UIGameplayRootViewModel : UIRootViewModel
{
    public UIGameplayRootViewModel(Subject<Unit> focusedEscapeRequest, Subject<Unit> focusedTabRequest)
        : base(focusedEscapeRequest, focusedTabRequest)
    {
    }
}
