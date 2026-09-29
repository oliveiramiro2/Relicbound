using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCapabilityController))]
public class RelicManager : MonoBehaviour
{
    private PlayerCapabilityController capabilityController;
    private RelicInventory inventory;
    private RelicEquipment equipment;
    public int RelicCount =>
    Relics.Count;

    public IReadOnlyList<Relic> Relics =>
        inventory.Relics;

    public IReadOnlyList<Relic> EquippedRelics =>
        equipment.EquippedRelics;

    public int RelicSlots =>
        equipment.SlotCount;

    private void Awake()
    {
        capabilityController =
            GetComponent<PlayerCapabilityController>();

        inventory = new RelicInventory();

        equipment = new RelicEquipment(
            2,
            capabilityController
        );
    }

    public bool AddRelic(Relic relic)
    {
        if (relic == null)
            return false;

        inventory.Add(relic);
        return true;
    }

    public bool RemoveRelic(Relic relic)
    {
        if (relic == null)
            return false;

        if (equipment.IsEquipped(relic))
            return false;

        return inventory.Remove(relic);
    }

    public bool EquipRelic(Relic relic)
    {
        if (relic == null)
            return false;

        if (!inventory.Contains(relic))
            return false;

        return equipment.Equip(relic);
    }

    public bool UnequipRelic(Relic relic)
    {
        if (relic == null)
            return false;

        return equipment.Unequip(relic);
    }

    public void AddRelicSlots(int amount)
    {
        equipment.AddSlots(amount);
    }
}