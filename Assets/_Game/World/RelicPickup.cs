using UnityEngine;

public class RelicPickup : MonoBehaviour
{
  [SerializeField]
  private RelicData relicData;

  public RelicData RelicData =>
      relicData;

  public void Initialize(RelicData data)
  {
    relicData = data;
  }

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

    if (relicData == null)
    {
      Debug.LogError(
          "RelicPickup: " +
          "No RelicData assigned."
      );

      return false;
    }

    bool collected =
        collector.Collect(relicData);

    if (!collected)
      return false;

    Destroy(gameObject);

    return true;
  }
}