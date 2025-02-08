using UnityEngine;

public class PlayerBinder : CreatureBinder
{
    private Vector2 _direction;
    private PlayerViewModel _viewModel;

    protected override void Start()
    {
        base.Start();


    }

    protected override void Update()
    {
        base.Update();

        _direction = _viewModel.MoveDirection.Value;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!movementBlocked)
            rb.linearVelocity = _viewModel.Stats.Speed.Value * _direction;
    }

    protected override void OnBind(CreatureViewModel viewModel)
    {
        var playerModel = (PlayerViewModel)viewModel;
        _viewModel = playerModel;


    }
}