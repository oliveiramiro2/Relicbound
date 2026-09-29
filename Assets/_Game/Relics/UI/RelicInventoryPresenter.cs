using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RelicManager))]
[RequireComponent(typeof(RelicLoadout))]
public class RelicInventoryPresenter : MonoBehaviour
{
  [SerializeField] private RelicDatabase database;

  [SerializeField] private RelicSlotView relicSlotPrefab;

  [SerializeField] private Transform inventoryContainer;
  [SerializeField] private Transform equipmentContainer;

  [SerializeField] private TMP_Text slotCountText;

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
    RefreshInventory();
    RefreshEquipment();
    RefreshSlotCount();
  }

  private void RefreshInventory()
  {
    ClearContainer(inventoryContainer);

    foreach (Relic relic in Relics)
    {
      RelicData data = GetData(relic);

      if (data == null)
        continue;

      CreateSlot(
          inventoryContainer,
          relic,
          data
      );
    }
  }

  private void RefreshEquipment()
  {
    ClearContainer(equipmentContainer);

    foreach (Relic relic in EquippedRelics)
    {
      RelicData data = GetData(relic);

      if (data == null)
        continue;

      CreateSlot(
          equipmentContainer,
          relic,
          data
      );
    }
  }

  private void RefreshSlotCount()
  {
    slotCountText.text =
        $"Slots: {EquippedRelics.Count} / {RelicSlots}";
  }

  private void CreateSlot(
      Transform container,
      Relic relic,
      RelicData data
  )
  {
    RelicSlotView slot =
        Instantiate(relicSlotPrefab, container);

    slot.Setup(
        relic,
        data,
        IsEquipped(relic)
    );

    slot.Clicked += OnRelicClicked;
  }

  private void ClearContainer(Transform container)
  {
    foreach (Transform child in container)
    {
      Destroy(child.gameObject);
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