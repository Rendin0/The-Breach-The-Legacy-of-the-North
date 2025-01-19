
using R3;

public class UIMainMenuRootViewModel : UIRootViewModel
{
    public UIMainMenuRootViewModel(Subject<Unit> focusedEscapeRequest, Subject<Unit> focusedTabRequest)
        : base(focusedEscapeRequest, focusedTabRequest)
    {
    }
}