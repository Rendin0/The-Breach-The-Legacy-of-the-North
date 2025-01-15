
using R3;
using UnityEngine;

public class CreatureViewModel
{
    private readonly CreatureEntityProxy _creatureEntity;
    private readonly CreaturesSerivce _creaturesSerivce;

    public readonly int CreatureId;
    public ReactiveProperty<Vector3> Position { get; }

    public CreatureViewModel(CreatureEntityProxy creatureEntity, CreaturesSerivce creaturesSerivce)
    {
        this._creatureEntity = creatureEntity;
        this._creaturesSerivce = creaturesSerivce;

        CreatureId = _creatureEntity.Id;
        Position = _creatureEntity.Position;
    }

}