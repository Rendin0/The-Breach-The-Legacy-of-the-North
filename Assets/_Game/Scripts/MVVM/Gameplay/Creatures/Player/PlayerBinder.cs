using R3;
using UnityEngine;

public class PlayerBinder : CreatureBinder
{
    private Vector2 _direction;
    private Camera _camera;
    protected bool movementBlocked = false;

    private PlayerViewModel _playerViewModel;
    public override CreatureViewModel ViewModel => _playerViewModel;

    protected override void Start()
    {
        base.Start();
        _camera = Camera.main;

    }

    protected void Update()
    {
        _direction = _playerViewModel.MoveDirection.Value;
        _camera.transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!movementBlocked)
            rb.linearVelocity = ViewModel.Stats.Speed.Value * _direction;
    }

    protected override void OnBind(CreatureViewModel viewModel)
    {
        _playerViewModel = viewModel as PlayerViewModel;

        _playerViewModel.MovementBlocked.Subscribe(b => movementBlocked = b);
    }
}