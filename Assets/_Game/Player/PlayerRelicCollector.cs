using UnityEngine;

[RequireComponent(typeof(RelicManager))]
public class PlayerRelicCollector : MonoBehaviour
{
  private RelicManager relicManager;

  [SerializeField] private RelicData relicData;

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
}