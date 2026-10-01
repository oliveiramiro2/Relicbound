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

  [Header("Generation")]
  [SerializeField]
  private int maximumAttemptsPerPlatform = 30;

  public int PlatformCount =>
      platformCount;

  public float MinimumPlatformSpacing =>
      minimumPlatformSpacing;

  public int MaximumAttemptsPerPlatform =>
      maximumAttemptsPerPlatform;
}