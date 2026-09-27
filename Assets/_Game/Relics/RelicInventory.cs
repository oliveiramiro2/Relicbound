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

    public bool Contains(string relicId)
    {
        foreach (Relic relic in relics)
        {
            if (relic.Id == relicId)
                return true;
        }

        return false;
    }
}