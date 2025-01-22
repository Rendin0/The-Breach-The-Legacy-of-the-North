
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentBinder : MonoBehaviour
{
    [SerializeField] private List<InventorySlotBinder> _equipment;
    [SerializeField] private Button _statsButton;
    [SerializeField] private Button _fastEquipButton;
    [SerializeField] private Button _fastUnequipButton;

    private PopupInventoryViewModel _viewModel;

    private Dictionary<EquipmentType, InventorySlotBinder> _equipmentMap = new();

    private void Start()
    {
        _fastUnequipButton.onClick.AddListener(OnFastUnequipButtonClick);
    }

    private void OnFastUnequipButtonClick()
    {
        _viewModel.FastUnequipRequest();
    }

    public void Bind(PopupInventoryViewModel viewModel)
    {
        for (int i = 0; i < Enum.GetValues(typeof(EquipmentType)).Length; i++)
        {
            _equipmentMap[(EquipmentType)i] = _equipment[i];
        }

        foreach (var equip in viewModel.Equipment)
        {
            _equipmentMap[equip.Key].Bind(equip.Value);
        }

        _viewModel = viewModel;
    }
}