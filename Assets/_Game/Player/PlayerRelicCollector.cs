using UnityEngine;

[RequireComponent(typeof(RelicManager))]
public class PlayerRelicCollector : MonoBehaviour
{
  private RelicManager relicManager;

  private void Awake()
  {
    relicManager = GetComponent<RelicManager>();
  }

  public bool Collect(RelicData relicData)
  {
    if (relicData == null)
      return false;

    Relic relic = relicData.CreateRuntimeRelic();

    return relicManager.AddRelic(relic);
  }

  public bool CollectSlotExpansion(int amount)
  {
    if (amount <= 0)
      return false;

    if (relicManager == null)
      return false;

    return relicManager.AddRelicSlots(amount);
  }
}