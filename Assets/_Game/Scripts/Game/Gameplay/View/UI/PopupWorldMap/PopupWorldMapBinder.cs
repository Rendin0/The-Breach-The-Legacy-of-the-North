using R3;
using UnityEngine;

public class PopupWorldMapBinder : PopupBinder<PopupWorldMapViewModel>
{
    [SerializeField] private WorldMapBinder _worldMap;

    protected override void OnBind(PopupWorldMapViewModel viewModel)
    {
        viewModel.Scale.Subscribe(s => _worldMap.SetScale(s));
    }
}