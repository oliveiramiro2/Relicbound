using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RelicManager))]
public class RelicInventoryPresenter : MonoBehaviour
{
  private RelicManager relicManager;

  public IReadOnlyList<Relic> Relics =>
      relicManager.Relics;

  public IReadOnlyList<Relic> EquippedRelics =>
      relicManager.EquippedRelics;

  public int RelicSlots =>
      relicManager.RelicSlots;

  private void Awake()
  {
    relicManager = GetComponent<RelicManager>();
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

  private void OnInventoryChanged(Relic relic)
  {
    Refresh();
  }

  private void Refresh()
  {
    // A UI será atualizada aqui.
  }
}