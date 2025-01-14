using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMainMenuBinder : WindowBinder<ScreenMainMenuViewModel>
{
    [SerializeField] private Button _btnPlay;
    [SerializeField] private Button _btnSettings;
    [SerializeField] private Button _btnExit;

    private void OnEnable()
    {
        _btnPlay.onClick.AddListener(OnPlayButtonClicked);
        _btnSettings.onClick.AddListener(OnSettingsButtonClicked);
        _btnExit.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnDisable()
    {
        _btnExit.onClick.RemoveAllListeners();
        _btnPlay.onClick.RemoveAllListeners();
        _btnSettings.onClick.RemoveAllListeners();
    }

    private void OnExitButtonClicked()
    {
        ViewModel.RequestExitGame();
    }

    private void OnSettingsButtonClicked()
    {
        ViewModel.RequestOpenPopupSettings();
    }

    private void OnPlayButtonClicked()
    {
        ViewModel.RequestPlay();
    }
}