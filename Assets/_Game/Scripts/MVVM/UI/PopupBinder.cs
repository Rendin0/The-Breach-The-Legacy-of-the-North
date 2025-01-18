using UnityEngine;
using UnityEngine.UI;

public abstract class PopupBinder<T> : WindowBinder<T> where T : WindowViewModel
{
    [SerializeField] protected Button _btnClose;
    [SerializeField] protected Button _btnCloseAlt;

    protected virtual void Start()
    {
        _btnClose?.onClick.AddListener(OnCloseButtonClick);
        _btnCloseAlt?.onClick.AddListener(OnCloseButtonClick);
    }

    protected virtual void OnDestroy()
    {
        _btnClose?.onClick.RemoveAllListeners();
        _btnCloseAlt?.onClick.RemoveAllListeners();
    }

    protected virtual void OnCloseButtonClick()
    {
        ViewModel.RequestClose();
    }
}