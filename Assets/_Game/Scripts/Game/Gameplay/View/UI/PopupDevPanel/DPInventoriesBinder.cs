using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DPInventoriesBinder : MonoBehaviour
{
    private DPInventoriesViewModel _viewModel;

    [SerializeField] private TMP_InputField _creatureIdInputField;
    [SerializeField] private TMP_InputField _slotsAmountInputField;
    [SerializeField] private Button _addInventoryButton;
    [SerializeField] private Button _addSlotsButton;

    private void Awake()
    {
        _addSlotsButton.onClick.AddListener(OnAddSlotsButtonClicked);
        _addInventoryButton.onClick.AddListener(OnAddInventoryButtonClicked);
    }

    private void OnAddSlotsButtonClicked()
    {
        if (int.TryParse(_creatureIdInputField.text, out int creatureId) && int.TryParse(_slotsAmountInputField.text, out int slotsAmount))
        {
            _viewModel.AddSlots(creatureId, slotsAmount);
        }
    }

    private void OnAddInventoryButtonClicked()
    {
        if (int.TryParse(_creatureIdInputField.text, out int creatureId) && int.TryParse(_slotsAmountInputField.text, out int slotsAmount))
        {
            _viewModel.AddInventory(creatureId, slotsAmount);
        }
    }

    private void OnDestroy()
    {
        _addSlotsButton.onClick.RemoveAllListeners();
        _addInventoryButton.onClick.RemoveAllListeners();
    }

    public void Bind(DPInventoriesViewModel viewModel)
    {
        _viewModel = viewModel;
    }

}