using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerGrapple))]

[RequireComponent(typeof(PlayerCapabilityController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float deceleration = 40f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    private PlayerCapabilityController capabilityController;
    private int jumpsUsed;

    [Header("Dash")]
    [SerializeField] private float dashImpulse = 14f;
    [SerializeField] private float dashDuration = 0.15f;

    private int dashesAvailable;
    private bool isDashing;
    private int facingDirection = 1;
    private float dashTimer;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private PlayerGrapple playerGrapple;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerGrapple = GetComponent<PlayerGrapple>();
        capabilityController = GetComponent<PlayerCapabilityController>();

        dashesAvailable = 1;
    }

    private void Update()
    {
        CheckGround();
        UpdateFacingDirection();
        HandleJump();
        HandleDash();
        UpdateDash();
    }

    private void FixedUpdate()
    {
        if (playerGrapple.IsGrappling)
            return;

        if (isDashing)
            return;

        HandleMovement();
    }

    private void UpdateFacingDirection()
    {
        if (playerInput.MoveInput.x > 0.01f)
            facingDirection = 1;
        else if (playerInput.MoveInput.x < -0.01f)
            facingDirection = -1;
    }

    private void HandleMovement()
    {
        float finalMoveSpeed = moveSpeed;

        foreach (MovementSpeedCapability capability in
            capabilityController.Capabilities.GetAll<MovementSpeedCapability>())
        {
            finalMoveSpeed *= capability.Multiplier;
        }

        float targetSpeed =
            playerInput.MoveInput.x * finalMoveSpeed;

        float speedDifference =
            targetSpeed - rb.linearVelocity.x;

        float accelerationRate =
            Mathf.Abs(targetSpeed) > 0.01f
                ? acceleration
                : deceleration;

        float movement =
            speedDifference * accelerationRate;

        rb.AddForce(Vector2.right * movement);
    }

    public void ResetMovementState()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            playerInput.StopMovement();
        }
    }

    private void HandleJump()
    {
        if (!playerInput.JumpPressed)
            return;

        if (isGrounded)
        {
            jumpsUsed = 0;
            PerformJump();
            return;
        }

        DoubleJumpCapability doubleJump =
            capabilityController.Capabilities.Get<DoubleJumpCapability>();

        if (doubleJump == null)
            return;

        if (jumpsUsed >= doubleJump.ExtraJumps)
            return;

        jumpsUsed++;
        PerformJump();
    }

    private void HandleDash()
    {
        if (!playerInput.DashPressed)
            return;

        if (dashesAvailable <= 0)
            return;

        PerformDash();
    }

    private void PerformJump()
    {
        float finalJumpForce = jumpForce;

        foreach (JumpHeightCapability capability in
            capabilityController.Capabilities.GetAll<JumpHeightCapability>())
        {
            finalJumpForce *= capability.Multiplier;
        }

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            finalJumpForce
        );
    }

    private void PerformDash()
    {
        dashesAvailable--;
        isDashing = true;
        dashTimer = dashDuration;

        float finalDashImpulse = dashImpulse;

        foreach (DashImpulseCapability capability in
            capabilityController.Capabilities.GetAll<DashImpulseCapability>())
        {
            finalDashImpulse *= capability.Multiplier;
        }

        Vector2 dashDirection = GetDashDirection();

        rb.linearVelocity +=
            dashDirection * finalDashImpulse;
    }

    private void UpdateDash()
    {
        if (!isDashing)
            return;

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            isDashing = false;

            if (isGrounded)
            {
                dashesAvailable = GetMaxDashes();
            }
        }
    }

    private Vector2 GetDashDirection()
    {
        Vector2 input = playerInput.MoveInput;

        if (!capabilityController.Capabilities.Has<DirectionalDashCapability>())
        {
            if (input.x > 0.01f)
                return Vector2.right;

            if (input.x < -0.01f)
                return Vector2.left;

            return facingDirection == 1
                ? Vector2.right
                : Vector2.left;
        }

        if (input.sqrMagnitude > 0.01f)
            return input.normalized;

        return facingDirection == 1
            ? Vector2.right
            : Vector2.left;
    }

    private void CheckGround()
    {
        bool wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (!wasGrounded && isGrounded)
        {
            jumpsUsed = 0;
            dashesAvailable = GetMaxDashes();
            isDashing = false;
        }
    }

    private int GetMaxDashes()
    {
        int maxDashes = 1;

        foreach (ExtraDashCapability capability in
            capabilityController.Capabilities.GetAll<ExtraDashCapability>())
        {
            maxDashes += capability.ExtraDashes;
        }

        return maxDashes;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}