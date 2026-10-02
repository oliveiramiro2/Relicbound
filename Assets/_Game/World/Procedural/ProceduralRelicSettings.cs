using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Procedural Relic Settings",
    fileName = "ProceduralRelicSettings"
)]
public class ProceduralRelicSettings : ScriptableObject
{
  [Header("Generation")]
  [SerializeField]
  private bool generateRelics = true;

  [SerializeField]
  private int minimumRelicsPerRoom = 1;

  [SerializeField]
  private int maximumRelicsPerRoom = 2;

  [Header("Placement")]
  [SerializeField]
  private float verticalOffset = 1f;

  [Header("Exploration")]
  [SerializeField]
  private bool preferBranches = true;

  [SerializeField]
  private float branchSelectionWeight = 2f;

  public bool GenerateRelics =>
      generateRelics;

  public int MinimumRelicsPerRoom =>
      Mathf.Max(
          0,
          minimumRelicsPerRoom
      );

  public int MaximumRelicsPerRoom =>
      Mathf.Max(
          MinimumRelicsPerRoom,
          maximumRelicsPerRoom
      );

  public float VerticalOffset =>
      Mathf.Max(
          0f,
          verticalOffset
      );

  public bool PreferBranches =>
      preferBranches;

  public float BranchSelectionWeight =>
      Mathf.Max(
          0f,
          branchSelectionWeight
      );
}