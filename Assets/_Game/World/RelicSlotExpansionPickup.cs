using UnityEngine;

public class RelicSlotExpansionPickup : MonoBehaviour
{
  [SerializeField]
  private int slotsToAdd = 1;

  public int SlotsToAdd =>
      slotsToAdd;

  private void OnTriggerEnter2D(Collider2D other)
  {
    PlayerRelicCollector collector =
        other.GetComponent<PlayerRelicCollector>();

    if (collector == null)
      return;

    Collect(collector);
  }

  public bool Collect(
      PlayerRelicCollector collector
  )
  {
    if (collector == null)
      return false;

    if (slotsToAdd <= 0)
    {
      Debug.LogError(
          "RelicSlotExpansionPickup: " +
          "SlotsToAdd must be greater than zero."
      );

      return false;
    }

    bool collected =
        collector.CollectSlotExpansion(slotsToAdd);

    if (!collected)
      return false;

    Destroy(gameObject);

    return true;
  }
}