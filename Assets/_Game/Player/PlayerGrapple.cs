using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(GrappleVisualizer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerGrapple : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float grappleDetectionRange = 8f;
    [SerializeField] private LayerMask grappleLayer;

    [Header("Swing")]
    [SerializeField] private float swingAcceleration = 8f;
    [SerializeField] private float maxSwingSpeed = 15f;

    private PlayerInput playerInput;
    private GrappleVisualizer visualizer;
    private GrapplePoint currentTarget;
    private GrappleRope rope;
    private Rigidbody2D rb;
    public bool IsGrappling => isGrappling;
    private float ropeLength;
    private bool isGrappling;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        visualizer = GetComponent<GrappleVisualizer>();
        rb = GetComponent<Rigidbody2D>();
        rope = GetComponentInChildren<GrappleRope>();
    }

    private void Update()
    {
        if (!isGrappling)
        {
            FindGrappleTarget();
        }

        if (playerInput.GrapplePressed)
        {
            if (isGrappling)
                ReleaseGrapple();
            else
                TryGrapple();
        }

        if (isGrappling)
        {
            UpdateRope();
        }
    }

    private void FixedUpdate()
    {
        if (!isGrappling)
            return;

        ApplyRopeConstraint();
        ApplySwingControl();
    }

    private void FindGrappleTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            grappleDetectionRange,
            grappleLayer
        );

        GrapplePoint closestPoint = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            GrapplePoint point = hit.GetComponent<GrapplePoint>();

            if (point == null)
                continue;

            float distance = Vector2.Distance(
                transform.position,
                point.Position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        currentTarget = closestPoint;

        if (currentTarget != null)
        {
            Debug.DrawLine(
                transform.position,
                currentTarget.Position
            );
        }

        if (currentTarget != null)
        {
            visualizer.ShowTarget(currentTarget.Position);
        }
        else
        {
            visualizer.HideTarget();
        }
    }

    private void TryGrapple()
    {
        if (currentTarget == null)
            return;

        isGrappling = true;

        ropeLength = Vector2.Distance(
            transform.position,
            currentTarget.Position
        );

        rope.Show(
            transform.position,
            currentTarget.Position
        );
    }

    private void UpdateRope()
    {
        rope.Show(
            transform.position,
            currentTarget.Position
        );
    }

    private void ReleaseGrapple()
    {
        isGrappling = false;
        rope.Hide();
    }

    private void ApplyRopeConstraint()
    {
        Vector2 playerPosition = rb.position;
        Vector2 pointPosition = currentTarget.Position;

        Vector2 toPlayer = playerPosition - pointPosition;

        float distance = toPlayer.magnitude;

        if (distance <= ropeLength)
            return;

        Vector2 direction = toPlayer.normalized;

        Vector2 constrainedPosition =
            pointPosition + direction * ropeLength;

        rb.position = constrainedPosition;

        float outwardVelocity = Vector2.Dot(
            rb.linearVelocity,
            direction
        );

        if (outwardVelocity > 0f)
        {
            rb.linearVelocity -= direction * outwardVelocity;
        }
    }

    private void ApplySwingControl()
    {
        float input = playerInput.MoveInput.x;

        if (Mathf.Abs(input) < 0.01f)
            return;

        Vector2 playerPosition = rb.position;
        Vector2 pointPosition = currentTarget.Position;

        Vector2 direction = (
            playerPosition - pointPosition
        ).normalized;

        Vector2 tangent = new Vector2(
            -direction.y,
            direction.x
        );

        Vector2 inputDirection = new Vector2(
            input,
            0f
        );

        float tangentialInput = Vector2.Dot(
            inputDirection,
            tangent
        );

        float tangentialVelocity = Vector2.Dot(
            rb.linearVelocity,
            tangent
        );

        float targetVelocity =
            tangentialVelocity +
            tangentialInput *
            swingAcceleration *
            Time.fixedDeltaTime;

        targetVelocity = Mathf.Clamp(
            targetVelocity,
            -maxSwingSpeed,
            maxSwingSpeed
        );

        Vector2 radialVelocity = direction *
            Vector2.Dot(
                rb.linearVelocity,
                direction
            );

        rb.linearVelocity =
            radialVelocity +
            tangent * targetVelocity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            grappleDetectionRange
        );
    }
}