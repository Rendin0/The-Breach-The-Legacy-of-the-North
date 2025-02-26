using UnityEngine;
using UnityEngine.UI;

public class ScreenMainMenuBinder : WindowBinder<ScreenMainMenuViewModel>
{
    [SerializeField] private Button _btnPlay;
    [SerializeField] private Button _btnSettings;
    [SerializeField] private Button _btnExit;
    [SerializeField] private Button _btnClear;

    private void OnEnable()
    {
        _btnPlay.onClick.AddListener(OnPlayButtonClicked);
        _btnSettings.onClick.AddListener(OnSettingsButtonClicked);
        _btnExit.onClick.AddListener(OnExitButtonClicked);
        _btnClear.onClick.AddListener(OnClearButtonClicked);
    }

    private void OnDisable()
    {
        _btnExit.onClick.RemoveAllListeners();
        _btnPlay.onClick.RemoveAllListeners();
        _btnSettings.onClick.RemoveAllListeners();
        _btnClear.onClick.RemoveAllListeners();
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

    private void OnClearButtonClicked()
    {
        PlayerPrefs.DeleteAll();
    }
}