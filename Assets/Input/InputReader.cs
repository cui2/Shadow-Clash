using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour
{
    public event Action<Vector2> OnMoveStarted;
    public event Action<Vector2> OnMove;
    public event Action OnJump;
    public event Action OnPunch;
    public event Action OnKick;
    public event Action OnSpecial;

    private GameInputs controls;
    private PlayerInput playerInput;

    private void Awake()
    {
        controls = new GameInputs();
        playerInput = GetComponent<PlayerInput>();

        // Mask inputs not coming from current control scheme
        controls.bindingMask = playerInput.currentControlScheme == "Gamepad" 
            ? InputBinding.MaskByGroup("Gamepad")
            : InputBinding.MaskByGroup("Keyboard&Mouse");
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.started += ctx => OnMoveStarted?.Invoke(ctx.ReadValue<Vector2>());
        controls.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);
        controls.Player.Jump.performed += ctx => OnJump?.Invoke();
        controls.Player.Punch.performed += ctx => OnPunch?.Invoke();
        controls.Player.Kick.performed += ctx => OnKick?.Invoke();
        controls.Player.Special.performed += ctx => OnSpecial?.Invoke();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public bool IsMoving => controls.Player.Move.ReadValue<Vector2>().magnitude > 0;
    public bool IsJumping => controls.Player.Jump.triggered;
    public bool IsPunching => controls.Player.Punch.triggered;
    public bool IsKicking => controls.Player.Kick.triggered;
    public bool IsSpecial => controls.Player.Special.triggered;
}
