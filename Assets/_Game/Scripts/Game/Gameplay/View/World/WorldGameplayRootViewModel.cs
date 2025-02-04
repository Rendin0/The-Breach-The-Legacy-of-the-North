using ObservableCollections;

public class WorldGameplayRootViewModel
{
    public readonly IObservableCollection<CreatureViewModel> CreatureViewModels;
    private readonly DIContainer _viewModelsContainer;

    public WorldGameplayRootViewModel(CreaturesSerivce creaturesSerivce, DIContainer viewModelContainer)
    {
        CreatureViewModels = creaturesSerivce.CreatureViewModels;

        _viewModelsContainer = viewModelContainer;
    }
}
