using ObservableCollections;

public class WorldGameplayRootViewModel
{
    public readonly IObservableCollection<CreatureViewModel> CreatureViewModels;
    private readonly DIContainer _viewModelsContainer;
    public readonly GOAPService GOAPService;
    public WorldGameplayRootViewModel(DIContainer viewModelContainer)
    {
        _viewModelsContainer = viewModelContainer;

        CreatureViewModels = _viewModelsContainer.Resolve<CreaturesSerivce>().CreatureViewModels;
        GOAPService = _viewModelsContainer.Resolve<GOAPService>();
    }
}
