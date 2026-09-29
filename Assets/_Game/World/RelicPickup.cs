using UnityEngine;

public class RelicPickup : MonoBehaviour
{
  [SerializeField] private RelicData relicData;

  private void OnTriggerEnter2D(Collider2D other)
  {
    PlayerRelicCollector collector =
        other.GetComponent<PlayerRelicCollector>();

    if (collector == null)
      return;

    Collect(collector);
  }

  public void Collect(PlayerRelicCollector collector)
  {
    if (relicData == null)
      return;

    bool collected = collector.Collect(relicData);

    if (!collected)
      return;

    Destroy(gameObject);
  }
}