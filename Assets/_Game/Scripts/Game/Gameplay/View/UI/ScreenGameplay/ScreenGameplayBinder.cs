using UnityEngine;

public class ScreenGameplayBinder : WindowBinder<ScreenGameplayViewModel>
{
    [SerializeField] private AbilitiesBarBinder _abilitiesBarBinder;
    [SerializeField] private PlayerStatsBinder _playerStatsBinder;

    protected override void OnBind(ScreenGameplayViewModel viewModel)
    {
        base.OnBind(viewModel);

        _playerStatsBinder.Bind(viewModel.playerStatsViewModel);
        _abilitiesBarBinder.Bind(viewModel.AbilitiesBarViewModel);
    }
}
