
using R3;
using UnityEngine;

public class CreatureViewModel
{
    private readonly CreatureEntityProxy _creatureEntity;
    private readonly CreatureConfig _creatureConfig;
    private readonly CreaturesSerivce _creaturesSerivce;

    public readonly int CreatureId;
    public readonly string TypeId;
    public ReactiveProperty<Vector3> Position { get; }

    public CreatureViewModel(CreatureEntityProxy creatureEntity, CreatureConfig creatureConfig, CreaturesSerivce creaturesSerivce)
    {
        this._creatureEntity = creatureEntity;
        this._creatureConfig = creatureConfig;
        this._creaturesSerivce = creaturesSerivce;

        TypeId = creatureConfig.TypeId;
        CreatureId = _creatureEntity.Id;
        Position = _creatureEntity.Position;
    }

}