using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Procedural Room Settings",
    fileName = "ProceduralRoomSettings"
)]
public class ProceduralRoomSettings : ScriptableObject
{
  [Header("Platforms")]
  [SerializeField]
  private int platformCount = 5;

  [SerializeField]
  private float minimumPlatformSpacing = 1f;

  [Header("Reachability")]
  [SerializeField]
  private float maximumHorizontalDistance = 5f;

  [SerializeField]
  private float maximumVerticalDistance = 2.5f;

  [SerializeField]
  private float minimumVerticalDistance = -2f;

  [Header("Generation")]
  [SerializeField]
  private int maximumAttemptsPerPlatform = 30;

  public int PlatformCount =>
      platformCount;

  public float MinimumPlatformSpacing =>
      minimumPlatformSpacing;

  public float MaximumHorizontalDistance =>
      maximumHorizontalDistance;

  public float MaximumVerticalDistance =>
      maximumVerticalDistance;

  public float MinimumVerticalDistance =>
      minimumVerticalDistance;

  public int MaximumAttemptsPerPlatform =>
      maximumAttemptsPerPlatform;
}