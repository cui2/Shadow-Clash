using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action OnJump;
    public event Action OnAttack;

    private GameInputs controls;
    private PlayerInput playerInput;

    private void Awake()
    {
        controls = new GameInputs();
        playerInput = GetComponent<PlayerInput>();

        // Mask inputs not coming from current control scheme
        controls.bindingMask = playerInput.currentControlScheme == "Gamepad" 
            ? InputBinding.MaskByGroup("Gamepad") // Not using gamepad
            : InputBinding.MaskByGroup("Keyboard&Mouse"); // Using gamepad
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);
        controls.Player.Jump.performed += ctx => OnJump?.Invoke();
        controls.Player.Attack.performed += ctx => OnAttack?.Invoke();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
