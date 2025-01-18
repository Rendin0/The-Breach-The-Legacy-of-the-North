using R3;
using UnityEngine;

public class PlayerBinder : CreatureBinder
{
    private Vector2 _direction;
    private PlayerViewModel _viewModel;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _direction = _viewModel.Direction.Value;
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + Time.fixedDeltaTime * _viewModel.Speed.Value * _direction);
        _viewModel.Position.OnNext(transform.position); 
    }

    protected override void OnBind(CreatureViewModel viewModel)
    {
        var playerModel = (PlayerViewModel)viewModel;
        _viewModel = playerModel;
    }
}