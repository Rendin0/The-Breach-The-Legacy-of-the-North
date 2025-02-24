
using R3;
using UnityEngine;

public interface IElementInfoViewModel
{
    public Sprite Icon { get; }
    public string ElementName { get; }
    public string Description { get; }

    public Subject<IElementInfoViewModel> OnMouseEnter { get; }
    public Subject<IElementInfoViewModel> OnMouseExit { get; }
}