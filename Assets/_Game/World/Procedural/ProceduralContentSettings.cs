using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Procedural Content Settings",
    fileName = "ProceduralContentSettings"
)]
public class ProceduralContentSettings : ScriptableObject
{
  [Header("Hazards")]
  [SerializeField]
  private bool generateHazards = true;

  [SerializeField]
  private int maximumHazardsPerRoom = 3;

  [SerializeField]
  private float minimumDistanceFromPlatformEdge = 0.75f;

  public bool GenerateHazards =>
      generateHazards;

  public int MaximumHazardsPerRoom =>
      maximumHazardsPerRoom;

  public float MinimumDistanceFromPlatformEdge =>
      minimumDistanceFromPlatformEdge;
}