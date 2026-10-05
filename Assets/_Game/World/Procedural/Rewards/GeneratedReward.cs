using UnityEngine;

public class GeneratedReward
{
  public ProceduralRewardType RewardType { get; }
  public Vector2 Position { get; }
  public GeneratedPlatform Platform { get; }

  public GeneratedReward(
      ProceduralRewardType rewardType,
      Vector2 position,
      GeneratedPlatform platform
  )
  {
    RewardType = rewardType;
    Position = position;
    Platform = platform;
  }
}