using R3;
using ObservableCollections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class PopupInventoryBinder : PopupBinder<PopupInventoryViewModel>
{
    [SerializeField] private List<InventorySlotBinder> _slots;
    [SerializeField] private Button _sortInventoryButton;
    [SerializeField] private Transform _selectedItemContainer;

    private CompositeDisposable _disposables = new();
    private InventorySlotBinder _selectedItem;
    private Vector3 _offset;

    protected override void Start()
    {
        _btnCloseAlt?.onClick.AddListener(OnExitButtonClicked);
        base.Start();
        _sortInventoryButton?.onClick.AddListener(OnSortButtonClicked);

    }

    private void Update()
    {
        if (_selectedItem != null)
            _selectedItem.transform.position = Input.mousePosition + _offset;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _sortInventoryButton.onClick.RemoveAllListeners();
        _disposables.Dispose();

    }
    private void OnSortButtonClicked()
    {
        ViewModel.RequestSortInventory();
    }

    private void OnExitButtonClicked()
    {
        ViewModel.RequestThrow();
    }

    protected override void OnBind(PopupInventoryViewModel viewModel)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].Bind(viewModel.Slots[i]);
        }

        _disposables.Add(
        viewModel.SelectedChanged.Subscribe(s =>
        {
            if (_selectedItem != null)
            {
                Destroy(_selectedItem.gameObject);
                _selectedItem = null;
            }

            if (s != -1 && viewModel.Slots[s].ItemId.Value != Items.Nothing)
            {
                _selectedItem = Instantiate(_slots[s], _selectedItemContainer);
                _selectedItem.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.6f);
                var rect = _selectedItem.GetComponent<RectTransform>();
                rect.sizeDelta = _slots[s].GetComponent<RectTransform>().sizeDelta;
                _offset = rect.sizeDelta * 1.1f;
            }
        }));
    }
}