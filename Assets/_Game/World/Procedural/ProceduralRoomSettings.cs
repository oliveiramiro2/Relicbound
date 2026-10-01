using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Procedural Room Settings",
    fileName = "ProceduralRoomSettings"
)]
public class ProceduralRoomSettings : ScriptableObject
{
  [Header("Platforms")]
  [SerializeField] private int platformCount = 5;

  [SerializeField] private float minimumPlatformSpacing = 1f;

  public int PlatformCount =>
      platformCount;

  public float MinimumPlatformSpacing =>
      minimumPlatformSpacing;
}