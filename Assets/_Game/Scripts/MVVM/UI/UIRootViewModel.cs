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

    private readonly CompositeDisposable _subs;
    private readonly InputRequests _inputRequests = new();

    public UIRootViewModel(InputRequests inputRequests)
    {
        _subs = inputRequests.Subscribe(_inputRequests);
    }

    public void OpenScreen(WindowViewModel screenViewModel)
    {
        _openedScreen.Value?.Dispose();
        _openedScreen.Value = screenViewModel;
        SetWindowBindings(screenViewModel);
    }

    public void OpenPopup(WindowViewModel popupViewModel)
    {
        WindowViewModel prevWindow = _openedScreen.Value;
        if (_openedPopups.Count > 0)
            prevWindow = _openedPopups[^1];


        if (_openedPopups.Contains(popupViewModel)) return;
        var sub = popupViewModel.CloseRequested.Subscribe(ClosePopup);
        _subscriptions.Add(popupViewModel, sub);
        _openedPopups.Add(popupViewModel);
        SetWindowBindings(popupViewModel);
    }

    public void ClosePopup(WindowViewModel popupViewModel)
    {
        if (!_openedPopups.Contains(popupViewModel)) return;

        popupViewModel.Dispose();
        _openedPopups.Remove(popupViewModel);

        var sub = _subscriptions[popupViewModel];
        sub?.Dispose();
        _subscriptions.Remove(popupViewModel);

        SetWindowBindings(_openedScreen.Value);
        foreach (var popup in _openedPopups)
        {
            SetWindowBindings(popup);
        }

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
        _subs.Dispose();
    }

    public void SetWindowBindings(WindowViewModel viewModel)
    {
        _inputRequests.SetRequests(viewModel.InputRequests);
    }

}