
public class PopupElementInfoViewModel : WindowViewModel
{
    public override string Id => "PopupElementInfo";

    public readonly string Text;

    public PopupElementInfoViewModel(IElementInfoViewModel element)
    {
        Text = element.Text;
    }
}
