using R3;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerBinder : CreatureBinder
{
    private Vector2 _direction;
    private Vector2 _lastMoveDirection;
    private Camera _camera;

    private Animator _animator;

    protected bool movementBlocked = false;

    private PlayerViewModel _playerViewModel;
    public override CreatureViewModel ViewModel => _playerViewModel;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

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

        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveMagnitude = rb.linearVelocity.magnitude;
        
        if (!movementBlocked)
        {
            rb.linearVelocity = ViewModel.Stats.Speed.Value * _direction;
        }
        _animator.SetFloat(Animator.StringToHash("MoveMagnitude"), moveMagnitude);

        if (moveMagnitude > 0.1f)
        {
            _animator.SetFloat(Animator.StringToHash("MoveX"), rb.linearVelocityX);
            _animator.SetFloat(Animator.StringToHash("MoveY"), rb.linearVelocityY);
            _lastMoveDirection = _direction;
            return;
        }

        _animator.SetFloat(Animator.StringToHash("LastMoveX"), _lastMoveDirection.x);
        _animator.SetFloat(Animator.StringToHash("LastMoveY"), _lastMoveDirection.y);
    }

    protected override void OnBind(CreatureViewModel viewModel)
    {
        _playerViewModel = viewModel as PlayerViewModel;

        _playerViewModel.MovementBlocked.Subscribe(b => movementBlocked = b);
    }
}