using System.Collections.Generic;

public class RelicEquipment
{
    private readonly List<Relic> equippedRelics = new();

    private readonly PlayerCapabilityController capabilityController;

    public IReadOnlyList<Relic> EquippedRelics => equippedRelics;

    public int SlotCount { get; private set; }

    public RelicEquipment(
        int initialSlots,
        PlayerCapabilityController capabilityController
    )
    {
        SlotCount = initialSlots;
        this.capabilityController = capabilityController;
    }

    public bool Equip(Relic relic)
    {
        if (relic == null)
            return false;

        if (equippedRelics.Count >= SlotCount)
            return false;

        if (equippedRelics.Contains(relic))
            return false;

        equippedRelics.Add(relic);

        relic.Apply(capabilityController);

        return true;
    }
}