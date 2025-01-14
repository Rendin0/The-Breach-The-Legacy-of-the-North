using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;

public class UIRootViewModel : IDisposable
{
    public ReadOnlyReactiveProperty<WindowViewModel> OpenedScreen => _openedScreen;
    public IObservableCollection<WindowViewModel> OpenedPopups => _openedPopups;

    private readonly ReactiveProperty<WindowViewModel> _openedScreen = new(null);
    private readonly ObservableList<WindowViewModel> _openedPopups = new();
    private readonly Dictionary<WindowViewModel, IDisposable> _subscriptions = new();


    public void OpenScreen(WindowViewModel screenViewModel)
    {
        _openedScreen.Value?.Dispose();
        _openedScreen.Value = screenViewModel;
    }

    public void OpenPopup(WindowViewModel popupViewModel)
    {
        if (_openedPopups.Contains(popupViewModel)) return;

        var sub = popupViewModel.CloseRequested.Subscribe(ClosePopup);
        _subscriptions.Add(popupViewModel, sub);


        _openedPopups.Add(popupViewModel);
    }

    public void ClosePopup(WindowViewModel popupViewModel)
    {
        if (!_openedPopups.Contains(popupViewModel)) return;

        popupViewModel.Dispose();
        _openedPopups.Remove(popupViewModel);

        var sub = _subscriptions[popupViewModel];
        sub?.Dispose();
        _subscriptions.Remove(popupViewModel);
    }

    public void ClosePopup(string id)
    {
        var openedPopupViewModel = _openedPopups.FirstOrDefault(p => p.Id == id);
        ClosePopup(openedPopupViewModel);
    }

    public void CloseAllPopups()
    {
        foreach (var popupViewModel in _openedPopups)
        {
            ClosePopup(popupViewModel);
        }
    }

    public void Dispose()
    {
        CloseAllPopups();
        _openedScreen.Dispose();
    }
}