using UnityEngine;

public class ProceduralReward : MonoBehaviour
{
  public ProceduralRewardType RewardType { get; private set; }

  public void Initialize(
      ProceduralRewardType rewardType
  )
  {
    RewardType = rewardType;
  }
}