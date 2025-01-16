using R3;
using ObservableCollections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PopupInventoryBinder : PopupBinder<PopupInventoryViewModel>
{
    [SerializeField] private List<InventorySlotBinder> _slots;
    [SerializeField] private Button _sortInventoryButton;

    protected override void Start()
    {
        base.Start();
        _sortInventoryButton.onClick.AddListener(OnSortButtonClicked);

    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        _sortInventoryButton.onClick.RemoveAllListeners();

    }
    private void OnSortButtonClicked()
    {
        
        ViewModel.RequestSortInventory();
    }

    protected override void OnBind(PopupInventoryViewModel viewModel)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].Bind(viewModel.Slots[i]);
        }
    }
}