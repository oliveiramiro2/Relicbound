using System.Collections.Generic;

public class RelicSaveService
{
  private readonly RelicManager relicManager;
  private readonly RelicDatabase relicDatabase;

  public RelicSaveService(
      RelicManager relicManager,
      RelicDatabase relicDatabase
  )
  {
    this.relicManager = relicManager;
    this.relicDatabase = relicDatabase;
  }

  public RelicSaveData Capture()
  {
    RelicSaveData data = new RelicSaveData
    {
      slotCount = relicManager.RelicSlots
    };

    foreach (Relic relic in relicManager.Relics)
    {
      data.inventoryRelicIds.Add(relic.Id);
    }

    foreach (Relic relic in relicManager.EquippedRelics)
    {
      data.equippedRelicIds.Add(relic.Id);
    }

    return data;
  }

  public bool Restore(RelicSaveData data)
  {
    if (data == null)
      return false;

    if (relicDatabase == null)
      return false;

    ClearCurrentState();

    RestoreSlots(data.slotCount);
    RestoreInventory(data.inventoryRelicIds);
    RestoreEquipment(data.equippedRelicIds);

    return true;
  }

  private void ClearCurrentState()
  {
    List<Relic> equippedRelics =
        new List<Relic>(relicManager.EquippedRelics);

    foreach (Relic relic in equippedRelics)
    {
      relicManager.UnequipRelic(relic);
    }

    List<Relic> inventoryRelics =
        new List<Relic>(relicManager.Relics);

    foreach (Relic relic in inventoryRelics)
    {
      relicManager.RemoveRelic(relic);
    }
  }

  private void RestoreSlots(int savedSlotCount)
  {
    int currentSlotCount = relicManager.RelicSlots;
    int slotsToAdd = savedSlotCount - currentSlotCount;

    if (slotsToAdd > 0)
    {
      relicManager.AddRelicSlots(slotsToAdd);
    }
  }

  private void RestoreInventory(
      List<string> relicIds
  )
  {
    foreach (string relicId in relicIds)
    {
      RelicData relicData =
          relicDatabase.GetById(relicId);

      if (relicData == null)
        continue;

      Relic relic =
          relicData.CreateRuntimeRelic();

      relicManager.AddRelic(relic);
    }
  }

  private void RestoreEquipment(
      List<string> equippedRelicIds
  )
  {
    foreach (string relicId in equippedRelicIds)
    {
      Relic relic =
          FindUnequippedRelic(relicId);

      if (relic == null)
        continue;

      relicManager.EquipRelic(relic);
    }
  }

  private Relic FindUnequippedRelic(string relicId)
  {
    foreach (Relic relic in relicManager.Relics)
    {
      if (relic.Id != relicId)
        continue;

      if (relicManager.IsRelicEquipped(relic))
        continue;

      return relic;
    }

    return null;
  }
}