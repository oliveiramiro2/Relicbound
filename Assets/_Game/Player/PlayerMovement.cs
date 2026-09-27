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
    }

    private void Update()
    {
        CheckGround();
        HandleJump();
    }

    private void FixedUpdate()
    {
        if (playerGrapple.IsGrappling)
            return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        float targetSpeed = playerInput.MoveInput.x * moveSpeed;

        float speedDifference = targetSpeed - rb.linearVelocity.x;

        float accelerationRate = Mathf.Abs(targetSpeed) > 0.01f
            ? acceleration
            : deceleration;

        float movement = speedDifference * accelerationRate;

        rb.AddForce(Vector2.right * movement);
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

    private void PerformJump()
    {
        float finalJumpForce = jumpForce;

        JumpHeightCapability jumpHeight =
            capabilityController.Capabilities.Get<JumpHeightCapability>();

        if (jumpHeight != null)
        {
            finalJumpForce *= jumpHeight.Multiplier;
        }

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            finalJumpForce
        );
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
        }
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