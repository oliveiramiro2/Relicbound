using UnityEngine;

public abstract class RelicData : ScriptableObject
{
    public abstract string Id { get; }

    public abstract Relic CreateRuntimeRelic();
}