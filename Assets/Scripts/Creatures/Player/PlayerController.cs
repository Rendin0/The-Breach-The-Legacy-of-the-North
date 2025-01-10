using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private IControllable controllable;
    private PlayerInput input;

    private void Awake()
    {
        input = new();
        input.Enable();

        controllable = GetComponent<IControllable>();
    }
    private void OnAttackPerformed(InputAction.CallbackContext obj)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        controllable.Use(mousePos);
    }

    private void Update()
    {
        ReadMovement();
    }

    private void ReadMovement()
    {
        var inputDir = input.Gameplay.Movement.ReadValue<Vector2>();

        controllable.Move(inputDir);
    }

    private void OnEnable()
    {
        input.Gameplay.Attack.performed += OnAttackPerformed;
    }
    private void OnDisable()
    {
        input.Gameplay.Attack.performed -= OnAttackPerformed;
        
    }
}
