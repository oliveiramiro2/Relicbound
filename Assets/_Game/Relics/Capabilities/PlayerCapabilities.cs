using System;
using System.Collections.Generic;

public class PlayerCapabilities
{
    private readonly List<PlayerCapability> capabilities = new();

    public void Add(PlayerCapability capability)
    {
        capabilities.Add(capability);
    }

    public bool Has<T>() where T : PlayerCapability
    {
        foreach (PlayerCapability capability in capabilities)
        {
            if (capability is T)
                return true;
        }

        return false;
    }

    public T Get<T>() where T : PlayerCapability
    {
        foreach (PlayerCapability capability in capabilities)
        {
            if (capability is T typedCapability)
                return typedCapability;
        }

        return null;
    }

    public IEnumerable<T> GetAll<T>()
    where T : PlayerCapability
    {
        foreach (PlayerCapability capability in capabilities)
        {
            if (capability is T typedCapability)
                yield return typedCapability;
        }
    }
}