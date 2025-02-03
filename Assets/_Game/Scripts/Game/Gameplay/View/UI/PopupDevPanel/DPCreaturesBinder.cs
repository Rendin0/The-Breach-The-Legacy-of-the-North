
using System;
using UnityEngine;
using UnityEngine.UI;

public class DPCreaturesBinder : MonoBehaviour
{
    private DPCreaturesViewModel _viewModel;

    [SerializeField] private Button _createButton;
    [SerializeField] private Button _privilegesButton;

    public void Bind(DPCreaturesViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    private void Awake()
    {
        _createButton.onClick.AddListener(OnCreateButtonClicked);
        _privilegesButton.onClick.AddListener(OnPrivilegesButtonClicked);
    }

    private void OnDestroy()
    {
        _createButton.onClick.RemoveAllListeners();
        _privilegesButton.onClick.RemoveAllListeners();
    }

    private void OnCreateButtonClicked()
    {
        _viewModel.ToggleCreateCreatureMode();
    }

    private void OnPrivilegesButtonClicked()
    {
        _viewModel.TogglePriveleges();
    }
}