public abstract class Relic
{
    public string Id { get; }

    protected Relic(string id)
    {
        Id = id;
    }

    public abstract void Apply(RelicContext context);

    public abstract void Remove(RelicContext context);
}