using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenGameplayPauseBinder : WindowBinder<ScreenGameplayPauseViewModel>
{
    [SerializeField] private Button _btnResume;
    [SerializeField] private Button _btnSettings;
    [SerializeField] private Button _btnReturnToMenu;

    private void OnEnable()
    {
        _btnResume.onClick.AddListener(OnResumeButtonClicked);
        _btnSettings.onClick.AddListener(OnSettingsButtonClicked);
        _btnReturnToMenu.onClick.AddListener(OnReturnToMenuButtonClicked);
    }

    private void OnDisable()
    {
        _btnResume.onClick.RemoveListener(OnResumeButtonClicked);
        _btnSettings.onClick.RemoveListener(OnSettingsButtonClicked);
        _btnReturnToMenu.onClick.RemoveListener(OnReturnToMenuButtonClicked);
    }

    private void OnResumeButtonClicked()
    {
        ViewModel.RequestResume();
    }

    private void OnSettingsButtonClicked()
    {
        ViewModel.RequestOpenPopupSettings();

    }

    private void OnReturnToMenuButtonClicked()
    {
        ViewModel.RequestReturnToMenu();

    }

}