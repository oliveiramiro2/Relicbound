using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCapabilityController))]
public class RelicManager : MonoBehaviour
{
    private PlayerCapabilityController capabilityController;
    private RelicInventory inventory;
    private RelicEquipment equipment;
    public event Action<Relic> RelicAdded;
    public event Action<Relic> RelicRemoved;
    public event Action<Relic> RelicEquipped;
    public event Action<Relic> RelicUnequipped;

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

        RelicAdded?.Invoke(relic);
        
        return true;
    }

    public bool RemoveRelic(Relic relic)
    {
        if (relic == null)
            return false;

        if (equipment.IsEquipped(relic))
            return false;

        bool removed = inventory.Remove(relic);

        if (!removed)
            return false;

        RelicRemoved?.Invoke(relic);

        return true;
    }

    public bool EquipRelic(Relic relic)
    {
        if (relic == null)
            return false;

        if (!inventory.Contains(relic))
            return false;

        bool equipped = equipment.Equip(relic);

        if (!equipped)
            return false;

        RelicEquipped?.Invoke(relic);

        return true;
    }

    public bool UnequipRelic(Relic relic)
    {
        if (relic == null)
            return false;

        bool unequipped = equipment.Unequip(relic);

        if (!unequipped)
            return false;

        RelicUnequipped?.Invoke(relic);

        return true;
    }

    public void AddRelicSlots(int amount)
    {
        equipment.AddSlots(amount);
    }
}