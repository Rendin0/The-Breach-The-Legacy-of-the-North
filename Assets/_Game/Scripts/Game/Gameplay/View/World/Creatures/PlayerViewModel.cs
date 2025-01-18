
using R3;
using UnityEngine;

public class PlayerViewModel : CreatureViewModel, IControllable
{
    public ReactiveProperty<Vector2> Direction { get; } = new();

    public PlayerViewModel(CreatureEntityProxy creatureEntity) 
        : base(creatureEntity)
    {

    }

    public void Move(Vector2 direction)
    {
        Direction.OnNext(direction);
    }
}