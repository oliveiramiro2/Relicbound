using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerGrapple : MonoBehaviour
{
    [SerializeField] private float grappleDetectionRange = 8f;

    private PlayerInput playerInput;

    private GrapplePoint currentTarget;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        FindGrappleTarget();
    }

    private void FindGrappleTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            grappleDetectionRange
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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            grappleDetectionRange
        );
    }
}