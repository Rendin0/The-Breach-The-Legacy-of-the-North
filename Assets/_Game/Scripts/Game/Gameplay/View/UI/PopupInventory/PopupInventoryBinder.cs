using R3;
using ObservableCollections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupInventoryBinder : PopupBinder<PopupInventoryViewModel>
{
    [SerializeField] private List<InventorySlotBinder> _slots; 


    protected override void Start()
    {
        base.Start();


    }

    protected override void OnDestroy()
    {
        base.OnDestroy();


    }

    protected override void OnBind(PopupInventoryViewModel viewModel)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].Bind(viewModel.Slots[i]);
        }
    }
}