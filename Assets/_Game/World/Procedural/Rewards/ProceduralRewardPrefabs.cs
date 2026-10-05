using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Procedural Reward Prefabs",
    fileName = "ProceduralRewardPrefabs"
)]
public class ProceduralRewardPrefabs : ScriptableObject
{
  [SerializeField]
  private GameObject relicPrefab;

  [SerializeField]
  private GameObject slotExpansionPrefab;

  public GameObject GetPrefab(
      ProceduralRewardType type
  )
  {
    return type switch
    {
      ProceduralRewardType.Relic =>
          relicPrefab,

      ProceduralRewardType.SlotExpansion =>
          slotExpansionPrefab,

      _ =>
          null
    };
  }
}