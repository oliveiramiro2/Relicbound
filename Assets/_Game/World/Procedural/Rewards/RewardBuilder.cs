using UnityEngine;

public class RewardBuilder
{
  private readonly Transform worldRoot;
  private readonly ProceduralRewardPrefabs prefabs;

  public RewardBuilder(
      Transform worldRoot,
      ProceduralRewardPrefabs prefabs
  )
  {
    this.worldRoot = worldRoot;
    this.prefabs = prefabs;
  }

  public void Build(
      RewardGenerationResult result
  )
  {
    if (result == null)
      return;

    if (worldRoot == null)
      return;

    if (prefabs == null)
      return;

    foreach (GeneratedReward reward
             in result.Rewards)
    {
      if (reward == null)
        continue;

      GameObject prefab =
          prefabs.GetPrefab(
              reward.RewardType
          );

      if (prefab == null)
        continue;

      Object.Instantiate(
          prefab,
          reward.Position,
          Quaternion.identity,
          worldRoot
      );
    }
  }
}