using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(GrappleVisualizer))]
public class PlayerGrapple : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float grappleDetectionRange = 8f;
    [SerializeField] private LayerMask grappleLayer;

    private PlayerInput playerInput;
    private GrappleVisualizer visualizer;
    private GrapplePoint currentTarget;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        visualizer = GetComponent<GrappleVisualizer>();
    }

    private void Update()
    {
        FindGrappleTarget();
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            grappleDetectionRange
        );
    }
}