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

    [Header("Connection Distance")]
    [SerializeField]
    private float minimumConnectionDistance = 4f;

    [SerializeField]
    private float maximumConnectionDistance = 10f;

    [Header("Grapple Point")]
    [SerializeField]
    private float minimumDistanceFromPlatform = 2f;

    [SerializeField]
    private float preferredDistanceFromPlatform = 3f;

    [SerializeField]
    private float maximumDistanceFromPlatform = 5f;

    [Header("Point Position")]
    [SerializeField]
    [Range(0f, 1f)]
    private float midpointInfluence = 0.75f;

    [SerializeField]
    private float verticalOffset = 1.5f;

    [Header("Candidate Evaluation")]
    [SerializeField]
    [Range(0f, 1f)]
    private float verticalMovementWeight = 0.4f;

    [SerializeField]
    [Range(0f, 1f)]
    private float horizontalMovementWeight = 0.2f;

    [SerializeField]
    [Range(0f, 1f)]
    private float distanceWeight = 0.25f;

    [SerializeField]
    [Range(0f, 1f)]
    private float routeDifferenceWeight = 0.35f;

    public bool Enabled =>
        enabled;

    public int MaximumConnections =>
        Mathf.Max(
            0,
            maximumConnections
        );

    public int MaximumConnectionsPerPlatform =>
        Mathf.Max(
            1,
            maximumConnectionsPerPlatform
        );

    public float MinimumConnectionDistance =>
        Mathf.Max(
            0f,
            minimumConnectionDistance
        );

    public float MaximumConnectionDistance =>
        Mathf.Max(
            MinimumConnectionDistance,
            maximumConnectionDistance
        );

    public float MinimumDistanceFromPlatform =>
        Mathf.Max(
            0f,
            minimumDistanceFromPlatform
        );

    public float PreferredDistanceFromPlatform =>
        Mathf.Max(
            MinimumDistanceFromPlatform,
            preferredDistanceFromPlatform
        );

    public float MaximumDistanceFromPlatform =>
        Mathf.Max(
            PreferredDistanceFromPlatform,
            maximumDistanceFromPlatform
        );

    public float MidpointInfluence =>
        Mathf.Clamp01(
            midpointInfluence
        );

    public float VerticalOffset =>
        verticalOffset;

    public float VerticalMovementWeight =>
        Mathf.Max(
            0f,
            verticalMovementWeight
        );

    public float HorizontalMovementWeight =>
        Mathf.Max(
            0f,
            horizontalMovementWeight
        );

    public float DistanceWeight =>
        Mathf.Max(
            0f,
            distanceWeight
        );

    public float RouteDifferenceWeight =>
        Mathf.Max(
            0f,
            routeDifferenceWeight
        );
}