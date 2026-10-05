using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Procedural Reward Settings",
    fileName = "ProceduralRewardSettings"
)]
public class ProceduralRewardSettings : ScriptableObject
{
  [Header("Generation")]
  [SerializeField] private bool enabled = true;
  [SerializeField] private int maximumRewardsPerRoom = 1;

  [Header("Reward Types")]
  [SerializeField] private bool allowRelics = true;
  [SerializeField] private bool allowSlotExpansions = true;

  [Header("Placement")]
  [SerializeField] private float verticalOffset = 1.5f;

  public bool Enabled =>
      enabled;

  public int MaximumRewardsPerRoom =>
      Mathf.Max(0, maximumRewardsPerRoom);

  public bool AllowRelics =>
      allowRelics;

  public bool AllowSlotExpansions =>
      allowSlotExpansions;

  public float VerticalOffset =>
      Mathf.Max(0f, verticalOffset);
}