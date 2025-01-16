
using R3;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotBinder : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _amount;

    public void Bind(InventorySlotViewModel viewModel)
    {
        viewModel.Amount.Subscribe(amount =>
        {
            _amount.text = (amount == 0 || amount == 1 ? "" : amount.ToString());
        });
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
    }
}