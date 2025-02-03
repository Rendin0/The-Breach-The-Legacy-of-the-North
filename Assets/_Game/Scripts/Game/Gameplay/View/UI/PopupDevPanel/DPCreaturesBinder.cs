
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DPCreaturesBinder : MonoBehaviour
{
    private DPCreaturesViewModel _viewModel;

    [SerializeField] private Button _createButton;
    [SerializeField] private Button _privilegesButton;
    [SerializeField] private TMP_Dropdown _creatureTypeDropdown;

    public void Bind(DPCreaturesViewModel viewModel)
    {
        _viewModel = viewModel;

        foreach (var creatureType in viewModel.CreatureTypesList)
        {
            _creatureTypeDropdown.options.Add(new(creatureType, null, Color.black));
        }
        _creatureTypeDropdown.RefreshShownValue();
    }

    private void Awake()
    {
        _createButton.onClick.AddListener(OnCreateButtonClicked);
        _privilegesButton.onClick.AddListener(OnPrivilegesButtonClicked);
        _creatureTypeDropdown.onValueChanged.AddListener(OnCreatureTypeChanger);
    }

    private void OnDestroy()
    {
        _createButton.onClick.RemoveAllListeners();
        _privilegesButton.onClick.RemoveAllListeners();
        _creatureTypeDropdown.onValueChanged.RemoveAllListeners();
    }

    private void OnCreateButtonClicked()
    {
        _viewModel.ToggleCreateCreatureMode();
    }

    private void OnPrivilegesButtonClicked()
    {
        _viewModel.TogglePriveleges();
    }

    private void OnCreatureTypeChanger(int index)
    {
        _viewModel.CurrentCreatureType = index;
    }
}