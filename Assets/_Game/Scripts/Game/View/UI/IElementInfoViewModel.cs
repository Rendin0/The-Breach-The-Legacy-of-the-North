
using R3;

public interface IElementInfoViewModel
{
    public string Text { get; }

    public Subject<IElementInfoViewModel> OnMouseEnter { get; }
    public Subject<IElementInfoViewModel> OnMouseExit { get; }
}