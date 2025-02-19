using R3;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupCreatureInfoBinder : PopupBinder<PopupCreatureInfoViewModel>
{
    [SerializeField] private TMP_Text _creatureNameText;
    [SerializeField] private TMP_Text _creatureIdText;

    [SerializeField] private Button _addSlotsButton;
    [SerializeField] private Button _addInventoryButton;
    [SerializeField] private TMP_InputField _slotsAmountInputField;

    [SerializeField] private Button _addItemButton;
    [SerializeField] private TMP_Dropdown _itemTypesDropdown;
    [SerializeField] private TMP_InputField _itemsAmountInputField;

    [SerializeField] private StorageBinder _storage;
    [SerializeField] private InventorySlotBinder _slotPrefab;

    [SerializeField] private TMP_Text _statsHealthText;
    [SerializeField] private TMP_Text _statsMaxHealthText;
    [SerializeField] private TMP_Text _statsSpeedText;
    [SerializeField] private TMP_Text _statsDamageText;

    private CompositeDisposable _subs = new();

    private void Awake()
    {
        _addSlotsButton.onClick.AddListener(OnAddSlotsButtonClicked);
        _addInventoryButton.onClick.AddListener(OnAddInventoryButtonClicked);
        _addItemButton.onClick.AddListener(OnAddItemButtonClicked);

        _storage.gameObject.SetActive(false);
    }

    private void OnAddItemButtonClicked()
    {
        if (int.TryParse(_itemsAmountInputField.text, out int amount))
        {
            ViewModel.AddItem(_itemTypesDropdown.value, amount);
        }
    }

    private void OnAddSlotsButtonClicked()
    {
        if (int.TryParse(_slotsAmountInputField.text, out int slotsAmount))
        {
            ViewModel.AddSlots(slotsAmount);
        }
    }

    private void OnAddInventoryButtonClicked()
    {
        if (int.TryParse(_slotsAmountInputField.text, out int slotsAmount))
        {
            if (ViewModel.AddInventory(slotsAmount))
            {
                _storage.gameObject.SetActive(true);
                var inventory = ViewModel.GetInventory();
                _storage.Bind(inventory.Storage, _slotPrefab);
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _addSlotsButton.onClick.RemoveAllListeners();
        _addInventoryButton.onClick.RemoveAllListeners();
        _addItemButton.onClick.RemoveAllListeners();
        _subs.Dispose();
    }

    protected override void OnBind(PopupCreatureInfoViewModel viewModel)
    {
        base.OnBind(viewModel);
        InitInfo(viewModel);
        InitInventory(viewModel);
        InitDropdown(viewModel);
        InitStats(viewModel);
    }

    private void InitStats(PopupCreatureInfoViewModel viewModel)
    {
        viewModel.CreatureViewModel.Stats.Health.Subscribe(h => _statsHealthText.text = $"Health: {h}").AddTo(_subs);
        viewModel.CreatureViewModel.Stats.MaxHealth.Subscribe(mh => _statsMaxHealthText.text = $"MaxHealth: {mh}").AddTo(_subs);
        viewModel.CreatureViewModel.Stats.Speed.Subscribe(s => _statsSpeedText.text = $"Speed: {s}").AddTo(_subs);
    }

    private void InitDropdown(PopupCreatureInfoViewModel viewModel)
    {
        var itemTypes = viewModel.ItemTypes;
        _itemTypesDropdown.AddOptions(itemTypes);
    }

    private void InitInventory(PopupCreatureInfoViewModel viewModel)
    {
        var inventory = viewModel.GetInventory();
        if (inventory != null)
        {
            _storage.gameObject.SetActive(true);
            _storage.Bind(inventory.Storage, _slotPrefab);
        }
    }

    private void InitInfo(PopupCreatureInfoViewModel viewModel)
    {
        _creatureNameText.text = viewModel.CreatureViewModel.TypeId;
        _creatureIdText.text = $"Id: {viewModel.CreatureViewModel.CreatureId}";
    }
}