using UnityEngine;

public abstract class RelicData : ScriptableObject
{
    public abstract string Id { get; }

    [Header("Presentation")]
    [SerializeField] private string displayName;
    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    public string DisplayName => displayName;
    public string Description => description;
    public Sprite Icon => icon;

    public abstract Relic CreateRuntimeRelic();
}