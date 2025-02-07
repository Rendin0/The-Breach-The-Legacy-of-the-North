
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatureViewModel
{
    private readonly CreatureEntityProxy _creatureEntity;

    public readonly int CreatureId;
    public readonly string TypeId;

    public Rigidbody2D Rb { get; set; }
    public ReactiveProperty<Vector2> Position { get; }
    public ReactiveProperty<float> Speed { get; }
    public ReactiveProperty<bool> MovementBlocked { get; } = new(false);

    public readonly Subject<CreatureViewModel> OnCreatureClick = new();
    public readonly Subject<CreatureViewModel> DeleteRequest = new();

    public CreatureViewModel(CreatureEntityProxy creatureEntity)
    {
        _creatureEntity = creatureEntity;

        TypeId = _creatureEntity.TypeId;
        CreatureId = _creatureEntity.Id;
        Position = _creatureEntity.Position;
        Speed = _creatureEntity.Speed;
    }

    public virtual void OnClick(PointerEventData eventData)
    {
        OnCreatureClick.OnNext(this);
    }
}