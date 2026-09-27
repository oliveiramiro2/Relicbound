public abstract class Relic
{
    public string Id { get; }

    protected Relic(string id)
    {
        Id = id;
    }

    public abstract void Apply(
        PlayerCapabilityController capabilityController
    );
}