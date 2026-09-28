public class SpeedRelic : Relic
{
    private MovementSpeedCapability capability;

    public SpeedRelic()
        : base("relic.speed")
    {
    }

    public override void Apply(RelicContext context)
    {
        capability = new MovementSpeedCapability(1.25f);

        context.CapabilityController.AddCapability(
            capability
        );
    }

    public override void Remove(RelicContext context)
    {
        context.CapabilityController.RemoveCapability(
            capability
        );

        capability = null;
    }
}