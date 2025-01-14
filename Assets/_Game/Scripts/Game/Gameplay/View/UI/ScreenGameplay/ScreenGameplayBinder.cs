using UnityEngine;
using UnityEngine.UI;

public class ScreenGameplayBinder : WindowBinder<ScreenGameplayViewModel>
{
    [SerializeField] private Button _btnPause;

    private void OnEnable()
    {
        _btnPause.onClick.AddListener(OnRequestPause);
    }

    private void OnDisable()
    {
        _btnPause.onClick.RemoveAllListeners();
    }

    public void OnRequestPause()
    {
        ViewModel.RequestPause();
    }

}
