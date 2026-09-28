using System.Collections.Generic;

public class RelicInventory
{
    private readonly List<Relic> relics = new();

    public IReadOnlyList<Relic> Relics => relics;

    public void Add(Relic relic)
    {
        if (relic == null)
            return;

        relics.Add(relic);
    }

    public bool Contains(Relic relic)
    {
        if (relic == null)
            return false;

        return relics.Contains(relic);
    }
}