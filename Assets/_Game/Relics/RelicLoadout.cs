using UnityEngine;

[RequireComponent(typeof(RelicManager))]
public class RelicLoadout : MonoBehaviour
{
  private RelicManager relicManager;

  private void Awake()
  {
    relicManager = GetComponent<RelicManager>();
  }

  public bool Equip(Relic relic)
  {
    return relicManager.EquipRelic(relic);
  }

  public bool Unequip(Relic relic)
  {
    return relicManager.UnequipRelic(relic);
  }
}