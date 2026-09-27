using System.Collections.Generic;

public class RelicEquipment
{
    private readonly List<Relic> equippedRelics = new();

    public IReadOnlyList<Relic> EquippedRelics => equippedRelics;

    public int SlotCount { get; private set; }

    public RelicEquipment(int initialSlots)
    {
        SlotCount = initialSlots;
    }

    public bool Equip(Relic relic)
    {
        if (relic == null)
            return false;

        if (equippedRelics.Count >= SlotCount)
            return false;

        equippedRelics.Add(relic);

        return true;
    }
}