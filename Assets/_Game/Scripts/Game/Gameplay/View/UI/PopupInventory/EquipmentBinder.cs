
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentBinder : MonoBehaviour
{
    [SerializeField] private List<InventorySlotBinder> _equipment;
    [SerializeField] private List<StarBinder> _starsSmall;
    [SerializeField] private StarBinder _starLarge;

    [SerializeField] private Button _statsButton;
    [SerializeField] private Button _fastEquipButton;
    [SerializeField] private Button _fastUnequipButton;

    private PopupInventoryViewModel _viewModel;

    private Dictionary<EquipmentType, InventorySlotBinder> _equipmentMap = new();
    private Dictionary<EquipmentType, StarBinder> _starsMap = new();

    private readonly CompositeDisposable _subs = new();

    private void Start()
    {
        _fastUnequipButton.onClick.AddListener(OnFastUnequipButtonClick);
    }

    private void OnDestroy()
    {
        _subs.Dispose();
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
            _starsMap[(EquipmentType)i] = _starsSmall[i];
        }

        foreach (var equip in viewModel.Equipment)
        {
            _equipmentMap[equip.Key].Bind(equip.Value);
            var star = _starsMap[equip.Key];
            star.Bind(equip.Value, viewModel.ItemsConfig);
            _subs.Add(star.StarChanged.Subscribe(_ => CalculateLargeStar()));
        }

        CalculateLargeStar();
        _viewModel = viewModel;
    }

    private void CalculateLargeStar()
    {
        bool allEquiped = true;
        float sum = 0;

        foreach (var star in _starsMap.Values)
        {
            sum += (int)star.Rarity;
            if (star.Rarity == ItemRarity.Nothing)
                allEquiped = false;
        }

        int avgRarity = -1;
        if (allEquiped)
        {
            avgRarity = Mathf.FloorToInt(sum / _starsMap.Count);
        }

        _starLarge.SetImage($"UI/Stars/StarLarge{(ItemRarity)avgRarity}");

    }
}