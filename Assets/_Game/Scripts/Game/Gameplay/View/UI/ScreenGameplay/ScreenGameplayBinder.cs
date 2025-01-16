using UnityEngine;
using UnityEngine.UI;

public class ScreenGameplayBinder : WindowBinder<ScreenGameplayViewModel>
{
    [SerializeField] private Button _btnPause;
    [SerializeField] private Button _btnInventory;

    private void OnEnable()
    {
        _btnPause.onClick.AddListener(OnRequestPause);
        _btnInventory.onClick.AddListener(OnRequestInventory);
    }

    private void OnDisable()
    {
        _btnPause.onClick.RemoveAllListeners();
        _btnInventory.onClick.RemoveAllListeners();
    }

    public void OnRequestPause()
    {
        ViewModel.RequestPause();
    }

    public void OnRequestInventory()
    {
        ViewModel.RequestInventory(0);
    }

}
