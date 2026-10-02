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

  [Header("Point Placement")]
  [SerializeField]
  private float distanceFromPlatform = 0.75f;

  public bool Enabled =>
      enabled;

  public int MaximumConnections =>
      maximumConnections;

  public float DistanceFromPlatform =>
      distanceFromPlatform;
}