using ObservableCollections;

public class WorldGameplayRootViewModel
{
    public readonly IObservableCollection<CreatureViewModel> CreatureViewModels;
    public WorldGameplayRootViewModel(CreaturesSerivce creaturesSerivce)
    {
        CreatureViewModels = creaturesSerivce.CreatureViewModels;
    }
}
