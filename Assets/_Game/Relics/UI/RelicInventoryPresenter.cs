using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RelicManager))]
[RequireComponent(typeof(RelicLoadout))]
public class RelicInventoryPresenter : MonoBehaviour
{
  [SerializeField] private RelicDatabase database;
  [SerializeField] private RelicSlotView relicSlotPrefab;
  [SerializeField] private Transform relicContainer;

  private RelicManager relicManager;
  private RelicLoadout relicLoadout;

  public IReadOnlyList<Relic> Relics =>
      relicManager.Relics;

  public IReadOnlyList<Relic> EquippedRelics =>
      relicManager.EquippedRelics;

  public int RelicSlots =>
      relicManager.RelicSlots;

  private void Awake()
  {
    relicManager = GetComponent<RelicManager>();
    relicLoadout = GetComponent<RelicLoadout>();
  }

  private void OnEnable()
  {
    relicManager.RelicAdded += OnInventoryChanged;
    relicManager.RelicRemoved += OnInventoryChanged;
    relicManager.RelicEquipped += OnInventoryChanged;
    relicManager.RelicUnequipped += OnInventoryChanged;
  }

  private void OnDisable()
  {
    relicManager.RelicAdded -= OnInventoryChanged;
    relicManager.RelicRemoved -= OnInventoryChanged;
    relicManager.RelicEquipped -= OnInventoryChanged;
    relicManager.RelicUnequipped -= OnInventoryChanged;
  }

  public RelicData GetData(Relic relic)
  {
    if (relic == null)
      return null;

    if (database == null)
      return null;

    return database.GetById(relic.Id);
  }

  private void OnInventoryChanged(Relic relic)
  {
    Refresh();
  }

  private void Refresh()
  {
    foreach (Transform child in relicContainer)
    {
      Destroy(child.gameObject);
    }

    foreach (Relic relic in Relics)
    {
      RelicData data = GetData(relic);

      if (data == null)
        continue;

      RelicSlotView slot =
          Instantiate(relicSlotPrefab, relicContainer);

      slot.Setup(
          relic,
          data,
          IsEquipped(relic)
      );

      slot.Clicked += OnRelicClicked;
    }
  }

  private void OnRelicClicked(Relic relic)
  {
    if (IsEquipped(relic))
    {
      relicLoadout.Unequip(relic);
      return;
    }

    relicLoadout.Equip(relic);
  }

  private bool IsEquipped(Relic relic)
  {
    foreach (Relic equippedRelic in EquippedRelics)
    {
      if (equippedRelic == relic)
        return true;
    }

    return false;
  }
}