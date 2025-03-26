using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Obsolete("Old map script")]
public class PopupWorldMapBinderOld : PopupBinder<PopupWorldMapViewModelOld>
{
    private MapImageBinder _currentImage;
    private MapImageBinder _worldMap;

    private void Awake()
    {
        _worldMap = Resources.Load<MapImageBinder>($"UI/WorldMap/WorldMap");
        LoadWorldMap();
    }

    protected override void OnBind(PopupWorldMapViewModelOld viewModel)
    {
        base.OnBind(viewModel);

        viewModel.InputRequests.EscapeRequest.Subscribe(c => RequestEscape(c, viewModel));

    }

    public void LoadImage(NonRectButton button)
    {
        var image = Resources.Load<MapImageBinder>($"UI/WorldMap/{button.name}");

        if (image == null)
        {
            Debug.LogError($"Can not find WorlMap Image with name {button.name}");
            return;
        }

        ChangeImage(image);
    }

    private void LoadWorldMap()
    {
        ChangeImage(_worldMap);
    }

    private void ChangeImage(MapImageBinder newImage)
    {
        var tmpImage = _currentImage;
        _currentImage = Instantiate(newImage, transform);
        _currentImage.Bind(this);

        if (tmpImage != null)
            Destroy(tmpImage.gameObject);
    }

    private void RequestEscape(InputAction.CallbackContext context, PopupWorldMapViewModelOld viewModel)
    {
        if (context.performed)
        {
            if (_currentImage.name == $"{_worldMap.name}(Clone)")
                viewModel.RequestClose();
            else
                LoadWorldMap();
        }
    }
}