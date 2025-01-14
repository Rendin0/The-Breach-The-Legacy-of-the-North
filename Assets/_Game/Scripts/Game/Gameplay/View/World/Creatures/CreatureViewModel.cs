
public class CreatureViewModel
{
    private readonly CreatureEntityProxy _creatureEntity;
    private readonly CreaturesSerivce _creaturesSerivce;

    public CreatureViewModel(CreatureEntityProxy creatureEntity, CreaturesSerivce creaturesSerivce)
    {
        this._creatureEntity = creatureEntity;
        this._creaturesSerivce = creaturesSerivce;
    }

}