using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    [SerializeField] private float grappleRadius = 0.5f;

    public Vector2 Position => transform.position;
    public float GrappleRadius => grappleRadius;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            grappleRadius
        );
    }
}