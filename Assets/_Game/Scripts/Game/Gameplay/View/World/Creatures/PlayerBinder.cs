using UnityEngine;

public class PlayerBinder : CreatureBinder
{
    private Vector2 _direction;
    private PlayerViewModel _viewModel;
    private Camera _camera;

    protected override void Start()
    {
        base.Start();
        _camera = Camera.main;

    }

    protected override void Update()
    {
        base.Update();

        _direction = _viewModel.MoveDirection.Value;
        _camera.transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0f);
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