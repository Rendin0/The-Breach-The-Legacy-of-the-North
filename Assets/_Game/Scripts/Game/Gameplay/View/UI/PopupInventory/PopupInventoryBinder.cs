using R3;
using ObservableCollections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class PopupInventoryBinder : PopupBinder<PopupInventoryViewModel>
{
    [SerializeField] InventoryBinder _inventory;
    [SerializeField] EquipmentBinder _equipment;

    protected override void Start()
    {
        _btnCloseAlt?.onClick.AddListener(OnExitButtonClicked);
        base.Start();

    }

    protected override void OnBind(PopupInventoryViewModel viewModel)
    {
        base.OnBind(viewModel);

        _inventory.Bind(viewModel);
        _equipment.Bind(viewModel);
    }


    private void OnExitButtonClicked()
    {
        ViewModel.RequestThrow();
    }
}