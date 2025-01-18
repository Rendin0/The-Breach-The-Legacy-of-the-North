
using R3;
using R3.Triggers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotBinder : Selectable, IPointerDownHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _amount;

    private InventorySlotViewModel _viewModel;
    private CompositeDisposable _subs = new();

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        _viewModel.RequestSelect();
    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _subs.Dispose();
    }

    public void Bind(InventorySlotViewModel viewModel)
    {
        var itemAmountChangedSub =
            viewModel.Amount.Subscribe(amount =>
            {
                _amount.text = (amount == 0 || amount == 1 ? "" : amount.ToString());
            });
        var itemIdChangedSub =
            viewModel.ItemId.Subscribe(id =>
            {
                var sprite = Resources.Load<Sprite>($"UI/Items/{id}");

                _image.sprite = sprite;

                if (sprite == null)
                {
                    _image.color = new Color(1, 1, 1, 0);
                    return;
                }
                _image.color = new Color(1, 1, 1, 1);
            });



        _subs.Add(itemAmountChangedSub);
        _subs.Add(itemIdChangedSub);

        _viewModel = viewModel;
    }

}