using R3;
using UnityEngine;

public class PopupWorldMapBinder : PopupBinder<PopupWorldMapViewModel>
{
    [SerializeField] private WorldMapBinder _worldMap;

    protected override void OnBind(PopupWorldMapViewModel viewModel)
    {
        _worldMap.Init(viewModel.Scale.Value);
        viewModel.Scale.Skip(1).Subscribe(s => _worldMap.SetScale(s));
    }
}