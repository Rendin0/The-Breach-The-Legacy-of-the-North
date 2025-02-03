
using System;
using UnityEngine;
using UnityEngine.UI;

public class DPCreaturesBinder : MonoBehaviour
{
    private DPCreaturesViewModel _viewModel;

    [SerializeField] private Button _createButton;

    public void Bind(DPCreaturesViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    private void Awake()
    {
        _createButton.onClick.AddListener(OnCreateButtonClicked);
    }

    private void OnDestroy()
    {
        _createButton.onClick.RemoveAllListeners();
    }

    private void OnCreateButtonClicked()
    {
        _viewModel.ToggleCreateCreatureMode();
    }
}