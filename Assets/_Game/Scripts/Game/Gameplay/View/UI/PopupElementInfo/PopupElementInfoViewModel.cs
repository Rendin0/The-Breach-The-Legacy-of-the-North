
using UnityEngine;

public class PopupElementInfoViewModel : WindowViewModel
{
    public override string Id => "PopupElementInfo";

    public readonly string Description;
    public readonly string ElementName;
    public readonly Sprite Icon;

    public PopupElementInfoViewModel(IElementInfoViewModel element)
    {
        ElementName = element.ElementName;
        Description = element.Description;
        Icon = element.Icon;
    }
}
