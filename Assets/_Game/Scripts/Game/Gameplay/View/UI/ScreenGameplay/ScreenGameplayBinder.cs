using UnityEngine;

public class ScreenGameplayBinder : WindowBinder<ScreenGameplayViewModel>
{
    [SerializeField] private AbilitiesBarBinder _abilitiesBarBinder;

    protected override void OnBind(ScreenGameplayViewModel viewModel)
    {
        base.OnBind(viewModel);

        _abilitiesBarBinder.Bind(viewModel.AbilitiesBarViewModel);
    }
}
