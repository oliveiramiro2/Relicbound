using UnityEngine;

public abstract class RelicData : ScriptableObject
{
    [SerializeField] private string id;

    public string Id => id;

    public abstract Relic CreateRuntimeRelic();
}