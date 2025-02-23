
using R3;
using UnityEngine;

public class InventorySlotViewModel : IElementInfoViewModel
{
    private readonly InventorySlot _origin;

    public ReactiveProperty<string> ItemId { get; }
    public ReactiveProperty<int> Amount { get; }

    public Sprite ItemIcon = null;
    public string ItemDescription;
    public ItemRarity Rarity;

    public Sprite Icon => ItemIcon;
    public string ElementName => ItemId.Value;
    public string Description => ItemDescription;


    private readonly Subject<IElementInfoViewModel> _onMouseEnter = new();
    private readonly Subject<IElementInfoViewModel> _onMouseExit = new();
    public Subject<IElementInfoViewModel> OnMouseEnter => _onMouseEnter;
    public Subject<IElementInfoViewModel> OnMouseExit => _onMouseExit;

    public readonly Subject<InventorySlotViewModel> SelectRequested = new();
    public readonly Subject<Unit> ResetColor = new();

    public void RequestSelect()
    {
        SelectRequested.OnNext(this);
    }
    public InventorySlotViewModel(InventorySlot origin)
    {
        _origin = origin;

        ItemId = origin.ItemId;
        Amount = origin.Amount;
    }
}