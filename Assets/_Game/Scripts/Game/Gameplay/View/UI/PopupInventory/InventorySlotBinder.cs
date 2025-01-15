
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
            if (id == null)
            {
                _image = null;
                return;
            }

            _image = Resources.Load<Image>($"UI/Items/{id}");
        });
    }
}