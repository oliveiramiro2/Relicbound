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

    public bool Remove(Relic relic)
    {
        if (relic == null)
            return false;

        return relics.Remove(relic);
    }

    public bool Contains(Relic relic)
    {
        if (relic == null)
            return false;

        return relics.Contains(relic);
    }

    public bool Contains(string relicId)
    {
        if (string.IsNullOrEmpty(relicId))
            return false;

        foreach (Relic relic in relics)
        {
            if (relic.Id == relicId)
                return true;
        }

        return false;
    }
}