using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Procedural Room Settings",
    fileName = "ProceduralRoomSettings"
)]
public class ProceduralRoomSettings : ScriptableObject
{
  [Header("Main Path")]
  [SerializeField]
  private int platformCount = 5;

  [Header("Branches")]
  [SerializeField]
  private int minimumBranches = 1;

  [SerializeField]
  private int maximumBranches = 2;

  [SerializeField]
  private int minimumBranchLength = 1;

  [SerializeField]
  private int maximumBranchLength = 3;

  [Header("Platform Spacing")]
  [SerializeField]
  private float minimumPlatformSpacing = 1f;

  [Header("Generation")]
  [SerializeField]
  private int maximumAttemptsPerPlatform = 30;

  public int PlatformCount =>
      platformCount;

  public int MinimumBranches =>
      minimumBranches;

  public int MaximumBranches =>
      maximumBranches;

  public int MinimumBranchLength =>
      minimumBranchLength;

  public int MaximumBranchLength =>
      maximumBranchLength;

  public float MinimumPlatformSpacing =>
      minimumPlatformSpacing;

  public int MaximumAttemptsPerPlatform =>
      maximumAttemptsPerPlatform;
}