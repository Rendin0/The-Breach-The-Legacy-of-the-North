using UnityEngine;

public abstract class WindowBinder<T> : MonoBehaviour, IWindowBinder where T : WindowViewModel
{
    protected T ViewModel;

    public void Bind(WindowViewModel viewModel)
    {
        ViewModel = (T)viewModel;

        OnBind(ViewModel);
    }

    public void Close()
    {
        BeforeClose();

        Destroy(gameObject);
    }

    protected virtual void BeforeClose() { }
    protected virtual void OnBind(T viewModel) { }

}