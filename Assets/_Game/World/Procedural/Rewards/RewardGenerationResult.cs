using System.Collections.Generic;

public class RewardGenerationResult
{
  private readonly List<GeneratedReward> rewards = new();

  public IReadOnlyList<GeneratedReward> Rewards =>
      rewards;

  public int Count =>
      rewards.Count;

  public void Add(GeneratedReward reward)
  {
    if (reward == null)
      return;

    rewards.Add(reward);
  }
}