using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool GrapplePressed { get; private set; }
    public bool DashPressed { get; private set; }

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Grapple.performed += OnGrapple;
        inputActions.Player.Dash.performed += OnDash;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Grapple.performed -= OnGrapple;
        inputActions.Player.Dash.performed -= OnDash;

        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        JumpPressed = true;
    }

    private void OnGrapple(InputAction.CallbackContext context)
    {
        GrapplePressed = true;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        DashPressed = true;
    }

    public void StopMovement()
    {
        MoveInput = Vector2.zero;
    }

    private void LateUpdate()
    {
        JumpPressed = false;
        GrapplePressed = false;
        DashPressed = false;
    }
}