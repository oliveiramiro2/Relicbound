using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Procedural Grapple Settings",
    fileName = "ProceduralGrappleSettings"
)]
public class ProceduralGrappleSettings : ScriptableObject
{
    [Header("Generation")]
    [SerializeField]
    private bool enabled = true;

    [SerializeField]
    private int maximumConnections = 3;

    [SerializeField]
    private int maximumConnectionsPerPlatform = 1;

    [Header("Distance")]
    [SerializeField]
    private float minimumGrappleDistance = 3f;

    [SerializeField]
    private float maximumGrappleDistance = 8f;

    [Header("Point Placement")]
    [SerializeField]
    private float distanceFromPlatform = 1.25f;

    [Header("Candidate Evaluation")]
    [SerializeField]
    [Range(0f, 1f)]
    private float preferredDistance = 0.65f;

    [SerializeField]
    [Range(0f, 1f)]
    private float verticalMovementWeight = 0.35f;

    [SerializeField]
    [Range(0f, 1f)]
    private float horizontalMovementWeight = 0.25f;

    [SerializeField]
    [Range(0f, 1f)]
    private float distanceWeight = 0.25f;

    [SerializeField]
    [Range(0f, 1f)]
    private float separationWeight = 0.15f;

    public bool Enabled =>
        enabled;

    public int MaximumConnections =>
        Mathf.Max(0, maximumConnections);

    public int MaximumConnectionsPerPlatform =>
        Mathf.Max(1, maximumConnectionsPerPlatform);

    public float MinimumGrappleDistance =>
        Mathf.Max(0f, minimumGrappleDistance);

    public float MaximumGrappleDistance =>
        Mathf.Max(
            MinimumGrappleDistance,
            maximumGrappleDistance
        );

    public float DistanceFromPlatform =>
        Mathf.Max(0f, distanceFromPlatform);

    public float PreferredDistance =>
        Mathf.Clamp01(preferredDistance);

    public float VerticalMovementWeight =>
        Mathf.Max(0f, verticalMovementWeight);

    public float HorizontalMovementWeight =>
        Mathf.Max(0f, horizontalMovementWeight);

    public float DistanceWeight =>
        Mathf.Max(0f, distanceWeight);

    public float SeparationWeight =>
        Mathf.Max(0f, separationWeight);
}